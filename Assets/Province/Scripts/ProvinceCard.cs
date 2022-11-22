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

        if(_selectedProvince != null)
            _selectedProvince.ProvinceUpdated -= ProvinceUpdated;
    }

    void ProvinceSelected(ProvinceController controller)
    {
        ClearCard();
        TrashCan.ShowTrashCan = true;
        _selectedProvince = controller;
        _selectedProvince.ProvinceUpdated += ProvinceUpdated;
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
        if(_selectedProvince != null)
            _selectedProvince.ProvinceUpdated -= ProvinceUpdated;

        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Money, 0));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.MoneyPerMonth, 0));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Population, 0));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.PeopleEmployeed, 0));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.UnemploymentPercentage, 0));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Environment, 0));
        ProvinceNameText.text = "Seleccione una provincia...";

        TrashCan.ShowTrashCan = false;

        foreach (ResourceItem currentItem in Resources)
            currentItem.gameObject.SetActive(false);
    }

    void ProvinceUpdated()
    {
        if(_selectedProvince != null)
            ProvinceSelected(_selectedProvince);
    }

    void SetKpis(ProvinceController controller)
    {
        ProvinceNameText.text = controller.Info.DisplayName;
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Money, controller.Money));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.MoneyPerMonth, controller.MonthlyExpenses));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Population, controller.Population));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.PeopleEmployeed, controller.PeopleEmployeed));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.UnemploymentPercentage, controller.UnemploymentPercentage));
        ProvinceEvents.GetInstance().OnKPIUpdated(new KPIUpdatedInfo(KPIName.Environment, controller.EnvironmentPoints));
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