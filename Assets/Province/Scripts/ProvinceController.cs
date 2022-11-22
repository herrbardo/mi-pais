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

public delegate void ProvinceUpdatedDelegate();

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
    [SerializeField] public int UnemploymentPercentage;
    [SerializeField] public int UnemploymentDangerousPercentage;

    public event ProvinceUpdatedDelegate ProvinceUpdated;

    bool isSelected;
    int turnsWithDangeoursUnemployment;
    Dictionary<NaturalResourceCode, int> resourcesUse;
    Dictionary<NaturalResourceCode, int> resourcesCooldown;
    

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
        GameEvents.GetInstance().TurnChanged += TurnChanged;

        this.Population = Info.Population;
        this.Money = Info.Money;
        this.MonthlyExpenses = Info.MonthlyExpenses;
        this.EnvironmentPoints = Info.EnvironmentPoints;
        this.State = ProvinceState.Working;
        this.UnemploymentDangerousPercentage = 80;
        this.UnemploymentPercentage = 100;
        resourcesUse = new Dictionary<NaturalResourceCode, int>();
        resourcesCooldown = new Dictionary<NaturalResourceCode, int>();
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
        OnProvinceUpdated();
    }

    void CalculateActivityImpactPerTurn()
    {
        int remainingPopulation = Population;
        PeopleEmployeed = 0;
        List<NaturalResource> usedResources = new List<NaturalResource>();

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
                usedResources.Add(currentResource);
            }
        }

        List<NaturalResource> resourcesNotUsed = Info.NaturalResources.Where(r => !usedResources.Where(u => u.Code == r.Code).Any()).ToList();
        RefreshCooldown(resourcesNotUsed);
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
            return resourcesUse[resource.Code] < resource.DurationInTurns;
        }
        else
            return true;
    }

    void CalculateUnemployment()
    {
        int unemployedPeople = Population - PeopleEmployeed;
        double aux = ((double) unemployedPeople) / ((double) Population);
        aux *= 100;
        UnemploymentPercentage = (int) aux;
        
        if(UnemploymentPercentage >= this.UnemploymentDangerousPercentage)
        {
            this.turnsWithDangeoursUnemployment++;

            if(this.turnsWithDangeoursUnemployment <= 3)
                MessagesEvents.GetInstance().OnMessagePublished(new MessageInfo(MessageType.Warning, string.Format("Desempleo preocupa en <b>{0}</b>.", Info.DisplayName)));
        }
        else
            this.turnsWithDangeoursUnemployment = 0;
    }

    void EvaluateProvinceState()
    {
        if(this.Money <= 0)
        {
            this.State = ProvinceState.Broke;
            MessagesEvents.GetInstance().OnMessagePublished(new MessageInfo(MessageType.Error, string.Format("La provincia <b>{0}</b> está en quiebra.", Info.DisplayName)));
        }
        else if(this.EnvironmentPoints <= 0)
        {
            this.State = ProvinceState.Wasted;
            MessagesEvents.GetInstance().OnMessagePublished(new MessageInfo(MessageType.Error, string.Format("<b>{0}</b> quedó inhabitable.", Info.DisplayName)));
        }
        else if(this.turnsWithDangeoursUnemployment > 3)
        {
            this.State = ProvinceState.Disbanded;
            MessagesEvents.GetInstance().OnMessagePublished(new MessageInfo(MessageType.Error, string.Format("No queda nadie, todos abandonaron <b>{0}</b> por falta de empleo.", Info.DisplayName)));
        }
    }

    void SetResourceUse(NaturalResource resource)
    {
        if(resourcesUse.ContainsKey(resource.Code))
                resourcesUse[resource.Code]++;
            else
                resourcesUse.Add(resource.Code, 1);
    }

    void RefreshCooldown(List<NaturalResource> resources)
    {
        foreach (NaturalResource currentResource in resources)
        {
            if(resourcesCooldown.ContainsKey(currentResource.Code))
            {
                int cooldown = resourcesCooldown[currentResource.Code];
                if(cooldown >= currentResource.CooldownInTurns)
                {
                    cooldown = 0;
                    if(resourcesUse.ContainsKey(currentResource.Code))
                        resourcesUse[currentResource.Code] = 0;
                }
                else
                    cooldown++;

                resourcesCooldown[currentResource.Code] = cooldown;
            }
            else
                resourcesCooldown.Add(currentResource.Code, 1);
        }
    }

    public int GetResourceUse(NaturalResourceCode code)
    {
        if(resourcesUse.ContainsKey(code))
            return resourcesUse[code];
        else
            return 0;
    }

    public int GetCooldown(NaturalResourceCode code)
    {
        if(resourcesCooldown.ContainsKey(code))
            return resourcesCooldown[code];
        else
            return 0;
    }

    void OnProvinceUpdated()
    {
        if(ProvinceUpdated != null)
            ProvinceUpdated();
    }
}
