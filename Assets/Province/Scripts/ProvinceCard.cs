using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class ProvinceCard : MonoBehaviour
{
    [SerializeField] TMP_Text ProvinceNameText;
    [SerializeField] TMP_Text MoneyText;
    [SerializeField] TMP_Text MoneyPerMonthText;
    [SerializeField] TMP_Text PopulationText;
    [SerializeField] TMP_Text EmployeesText;
    [SerializeField] TMP_Text EnvironmentPointsText;
    [SerializeField] TMP_Text StateText;
    [SerializeField] TMP_Text NaturalResourcesText;
    [SerializeField] TMP_Text ArrastrarAquiText;
    [SerializeField] ActivityContainer Container;
    [SerializeField] ProvinceLostContainer ProvinceLostContainer;

    ProvinceController _selectedProvince;

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
        ProvinceEvents.GetInstance().ActivityAssigned += ActivityAssigned;
        GameEvents.GetInstance().TurnChanged += TurnChanged;
    }
    
    private void Start()
    {
        ProvinceNameText.text = NaturalResourcesText.text = string.Empty;
        ArrastrarAquiText.enabled = false;
        ProvinceLostContainer.Hide();
        ClearCard();
    }

    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().ProvinceSelected -= ProvinceSelected;
        ProvinceEvents.GetInstance().ActivityAssigned -= ActivityAssigned;
        GameEvents.GetInstance().TurnChanged -= TurnChanged;
    }

    void ProvinceSelected(ProvinceController controller)
    {
        _selectedProvince = controller;

        ProvinceNameText.text = controller.Info.DisplayName;
        MoneyText.text = "Dinero: $" + controller.Money.ToString("N0");
        MoneyPerMonthText.text = "Gastos por turno: $" + controller.MonthlyExpenses.ToString("N0");
        PopulationText.text = "Población: " + controller.Population.ToString("N0");
        EmployeesText.text = "Empleados: " + controller.PeopleEmployeed.ToString("N0");
        EnvironmentPointsText.text = "Ambiente: " + controller.EnvironmentPoints.ToString("N0");
        StateText.text = "Estado: " + GetStateName(controller.State);

        ArrastrarAquiText.enabled = true;
        NaturalResourcesText.text = "Recursos: ";
        bool oneResourceAlreadyAdded = false;
        Container.DisableAllActivities();

        foreach (NaturalResource item in controller.Info.NaturalResources)
        {
            if(oneResourceAlreadyAdded)
                NaturalResourcesText.text += ", ";

            NaturalResourcesText.text += item.Name;
            oneResourceAlreadyAdded = true;
        }
        
        foreach (Activity item in controller.Activities)
            Container.EnableActivity(item.Name);

        ProvinceLostContainer.ShowState(controller.State);
    }

    void ActivityAssigned(Activity activity)
    {
        if(_selectedProvince == null || _selectedProvince.Activities.Where(a => a.Name.Equals(activity.Name)).Any())
            return;
        
        _selectedProvince.Activities.Add(activity);
        ProvinceSelected(_selectedProvince);
    }

    void ClearCard()
    {
        ProvinceNameText.text = "Seleccione una provincia...";
        NaturalResourcesText.text = string.Empty;
        MoneyText.text = "$0";
        MoneyPerMonthText.text = "$0";
        PopulationText.text = "$0";
        EmployeesText.text = "$0";
        EnvironmentPointsText.text = string.Empty;
        StateText.text = string.Empty;
    }

    void TurnChanged(int turn)
    {
        if(_selectedProvince != null)
            ProvinceSelected(_selectedProvince);
    }

    string GetStateName(ProvinceState state)
    {
        switch(state)
        {
            case ProvinceState.Broke:
                return "En quiebra";
            
            case ProvinceState.Wasted:
                return "Arruinada";

            case ProvinceState.Disbanded:
                return "Desbandada";

            case ProvinceState.Working:
            default:
                return "Funcionando";
        }
    }
}
