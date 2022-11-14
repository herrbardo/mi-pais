using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ProvinceState
{
    Working,
    Broke,
    Wasted,
    Disbanded
}

public class ProvinceController : MonoBehaviour
{
    [SerializeField] SpriteRenderer SelectedTickSprite;
    [SerializeField] public ProvinceInfo Info;
    [SerializeField] public List<Activity> Activities;

    [Header("KPIs")]
    [SerializeField] public int Population;
    [SerializeField] public int PeopleEmployeed;
    [SerializeField] public int Money;
    [SerializeField] public int MonthlyExpenses;
    [SerializeField] public int EnvironmentPoints;
    [SerializeField] public ProvinceState State;

    bool isSelected;
    int turnsWithDangeoursUnemployment;
    int unemploymentDangerousPercentage;
    Dictionary<NaturalResourceCode, int> resourcesUse;
    

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
        GameEvents.GetInstance().TurnChanged += TurnChanged;

        this.Population = Info.Population;
        this.Money = Info.Money;
        this.MonthlyExpenses = Info.MonthlyExpenses;
        this.EnvironmentPoints = Info.EnvironmentPoints;
        this.State = ProvinceState.Working;
        this.unemploymentDangerousPercentage = 80;
        resourcesUse = new Dictionary<NaturalResourceCode, int>();
    }

    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().ProvinceSelected -= ProvinceSelected;
        GameEvents.GetInstance().TurnChanged -= TurnChanged;
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
            if(!isSelected)
                SwitchSelection(true);
    }

    void SwitchSelection(bool select)
    {
        isSelected = select;
        SelectedTickSprite.gameObject.SetActive(select);

        if(select)
            ProvinceEvents.GetInstance().OnProvinceSelected(this);
    }

    void ProvinceSelected(ProvinceController controller)
    {
        if(!controller.Info.DisplayName.Equals(this.Info.DisplayName) && isSelected)
            SwitchSelection(false);
    }

    void TurnChanged(int turn)
    {
        if(State != ProvinceState.Working)
            return;

        this.Money -= Info.MonthlyExpenses;
        CalculateActivityImpactPerTurn();
        CalculateUnemployment();
        EvaluateProvinceState();
    }

    void CalculateActivityImpactPerTurn()
    {
        int remainingPopulation = Population;
        PeopleEmployeed = 0;

        foreach (Activity currentActivity in Activities)
        {
            List<NaturalResource> resourcesForCurrentActivity = Info.NaturalResources.Where(r => currentActivity.CompatibleResources.Where(c => c == r.Code).Any()).ToList();

            foreach (NaturalResource currentResource in resourcesForCurrentActivity)
            {
                if(!ResourceIsAvailable(currentResource))
                    continue;
                
                this.EnvironmentPoints -= currentActivity.EnvironmentImpactPerTurn;
                this.PeopleEmployeed += currentActivity.AmountEmployees;
                this.Money += CalculateIncome(currentActivity, ref remainingPopulation);
                SetResourceUse(currentResource);
            }
        }
    }

    int CalculateIncome(Activity activity, ref int remainingPopulation)
    {
        int income = 0;
        if(remainingPopulation <= 0)
            return 0;
        else if(activity.AmountEmployees >= remainingPopulation)
            remainingPopulation = 0;
        else
            remainingPopulation -= activity.AmountEmployees;

        income += activity.AmountEmployees * activity.IncomePerPerson;
        return income;
    }

    bool ResourceIsAvailable(NaturalResource resource)
    {
        if(resourcesUse.ContainsKey(resource.Code))
        {
            return resourcesUse[resource.Code] <= resource.DurationInTurns;
        }
        else
            return true;
    }

    void CalculateUnemployment()
    {
        int unemployedPeople = Population - PeopleEmployeed;
        decimal unemploymentPercentage = unemployedPeople / Population;
        unemploymentPercentage *= 100;

        if(unemploymentPercentage >= this.unemploymentDangerousPercentage)
            this.turnsWithDangeoursUnemployment++;
    }

    void EvaluateProvinceState()
    {
        if(this.Money <= 0)
            this.State = ProvinceState.Broke;
        else if(this.EnvironmentPoints <= 0)
            this.State = ProvinceState.Wasted;
        else if(this.turnsWithDangeoursUnemployment > 3)
            this.State = ProvinceState.Disbanded;
    }

    void SetResourceUse(NaturalResource resource)
    {
        if(resourcesUse.ContainsKey(resource.Code))
                resourcesUse[resource.Code]++;
            else
                resourcesUse.Add(resource.Code, 1);
    }
}
