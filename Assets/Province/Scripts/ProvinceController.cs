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
            if(ExistsResourceForThisActivity(currentActivity))
            {
                this.EnvironmentPoints -= currentActivity.EnvironmentImpactPerTurn;
                this.PeopleEmployeed += currentActivity.AmountEmployees;
                this.Money += CalculateIncome(currentActivity, ref remainingPopulation);
            }
        }
    }

    int CalculateIncome(Activity activity, ref int remainingPopulation)
    {
        List<NaturalResource> resources = Info.NaturalResources.Where(r => activity.CompatibleResources.Where(c => c == r.Code).Any()).ToList();
        int income = 0;
        foreach (NaturalResource currentResource in resources)
        {
            if(remainingPopulation <= 0)
                break;
            else if(activity.AmountEmployees >= remainingPopulation)
                remainingPopulation = 0;
            else
                remainingPopulation -= activity.AmountEmployees;

            income += activity.AmountEmployees * activity.IncomePerPerson;
        }

        return income;
    }

    bool ExistsResourceForThisActivity(Activity activity)
    {
        return Info.NaturalResources.Where(r => activity.CompatibleResources.Where(c => c == r.Code).Any()).Any();
    }

    void CalculateUnemployment()
    {
        double unemploymentPercentage = (Population - PeopleEmployeed) / Population * 100;
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
}
