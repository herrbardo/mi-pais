using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class ProvinceCard : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text ProvinceNameText;
    [SerializeField] TMP_Text MoneyText;
    [SerializeField] TMP_Text MoneyPerMonthText;
    [SerializeField] TMP_Text PopulationText;
    [SerializeField] TMP_Text EmployeesText;
    [SerializeField] TMP_Text EnvironmentPointsText;
    [SerializeField] TMP_Text StateText;
    [SerializeField] TMP_Text NaturalResourcesText;
    [SerializeField] TMP_Text ArrastrarAquiText;

    [Header("Objects")]
    [SerializeField] ActivityContainer Container;
    [SerializeField] ProvinceLostContainer ProvinceLostContainer;
    [SerializeField] GameObject ResourcePrefab;
    [SerializeField] GameObject ResourceContainer;
    [SerializeField] List<ResourceItem> Resources;
    [SerializeField] TrashCan TrashCan;

    ProvinceController _selectedProvince;

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
        ProvinceEvents.GetInstance().ActivityAssigned += ActivityAssigned;
        ProvinceEvents.GetInstance().ActivityUnassigned += ActivityUnassigned;
        GameEvents.GetInstance().TurnChanged += TurnChanged;
    }
    
    private void Start()
    {
        ProvinceNameText.text = string.Empty;
        ArrastrarAquiText.enabled = false;
        ProvinceLostContainer.Hide();
        ClearCard();
    }

    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().ProvinceSelected -= ProvinceSelected;
        ProvinceEvents.GetInstance().ActivityAssigned -= ActivityAssigned;
        ProvinceEvents.GetInstance().ActivityUnassigned -= ActivityUnassigned;
        GameEvents.GetInstance().TurnChanged -= TurnChanged;
    }

    void ProvinceSelected(ProvinceController controller)
    {
        ClearCard();
        TrashCan.ShowTrashCan = true;
        _selectedProvince = controller;
        ArrastrarAquiText.enabled = true;
        SetKpis(controller);
        ShowNaturalResources(controller.Info.NaturalResources);

        Container.DisableAllActivities();
        foreach (Activity item in controller.Activities)
            Container.EnableActivity(item.Name);

        foreach (ResourceItem currentResource in Resources)
            currentResource.ProvinceController = _selectedProvince;

        ProvinceLostContainer.ShowState(controller.State);
    }

    void ActivityAssigned(Activity activity)
    {
        if(_selectedProvince == null || _selectedProvince.Activities.Where(a => a.Name.Equals(activity.Name)).Any())
            return;
        
        _selectedProvince.Activities.Add(activity);
        ProvinceSelected(_selectedProvince);
    }

    void ActivityUnassigned(Activity activity)
    {
        if(_selectedProvince == null)
            return;

        Activity activityToRemove = _selectedProvince.Activities.Where(a => a.Name.Equals(activity.Name)).FirstOrDefault();
        if(activityToRemove == null)
            return;

        _selectedProvince.Activities.Remove(activityToRemove);
        ProvinceSelected(_selectedProvince);
    }

    void ClearCard()
    {
        ProvinceNameText.text = "Seleccione una provincia...";
        MoneyText.text = "$0";
        MoneyPerMonthText.text = "$0";
        PopulationText.text = "$0";
        EmployeesText.text = "$0";
        EnvironmentPointsText.text = string.Empty;
        StateText.text = string.Empty;
        TrashCan.ShowTrashCan = false;

        foreach (ResourceItem currentItem in Resources)
            currentItem.gameObject.SetActive(false);
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

    void SetKpis(ProvinceController controller)
    {
        ProvinceNameText.text = controller.Info.DisplayName;
        MoneyText.text = "Dinero: $" + controller.Money.ToString("N0");
        MoneyPerMonthText.text = "Gastos por turno: $" + controller.MonthlyExpenses.ToString("N0");
        PopulationText.text = "Población: " + controller.Population.ToString("N0");
        EmployeesText.text = "Empleados: " + controller.PeopleEmployeed.ToString("N0");
        EnvironmentPointsText.text = "Ambiente: " + controller.EnvironmentPoints.ToString("N0");
        StateText.text = "Estado: " + GetStateName(controller.State);
    }

    void ShowNaturalResources(List<NaturalResource> naturalResources)
    {
        foreach (NaturalResource item in naturalResources)
        {
            ResourceItem localResourceItem = Resources.Where(r => r.ResourceInfo.Code == item.Code).FirstOrDefault();
            if(localResourceItem != null)
                localResourceItem.gameObject.SetActive(true);
        }

        HorizontalLayoutGroup layoutGroup = ResourceContainer.GetComponent<HorizontalLayoutGroup>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
}