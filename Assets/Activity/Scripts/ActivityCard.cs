using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivityCard : MonoBehaviour
{
    [Header("Activity Components")]
    [SerializeField] TMP_Text ActivityNameText;
    [SerializeField] TMP_Text IncomePerPersonText;
    [SerializeField] TMP_Text EmployeesText;
    [SerializeField] List<ResourceItem> Resources;

    private void Awake()
    {
        Hide();
        UIEvents.GetInstance().MouseActivityAction += MouseActivityAction;
    }
    private void OnDestroy()
    {
        UIEvents.GetInstance().MouseActivityAction -= MouseActivityAction;
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }

    void Show()
    {
        this.gameObject.SetActive(true);
    }

    void MouseActivityAction(Activity item, bool enter)
    {
        if(enter)
        {
            Show();
            FillWithActivityData(item);
        }
        else
            Hide();
    }

    void FillWithActivityData(Activity activity)
    {
        ActivityNameText.text = activity.Name;
        IncomePerPersonText.text = activity.IncomePerPerson.ToString("N0");
        EmployeesText.text = activity.AmountEmployees.ToString("N0");
        
        foreach (ResourceItem currentResource in Resources)
        {
            bool resourceIsCompatibleWithActivity = activity.CompatibleResources.Contains(currentResource.ResourceInfo.Code);
            currentResource.gameObject.SetActive(resourceIsCompatibleWithActivity);
        }
    }
}
