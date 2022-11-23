using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum KPIName
{
    Money,
    MoneyPerMonth,
    Population,
    PeopleEmployeed,
    UnemploymentPercentage,
    Environment
}

public class KpiItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Data")]
    [SerializeField] Sprite Logo;
    [SerializeField] KPIName KPIName;
    [SerializeField] string TooltipValue;

    [Header("Components")]
    [SerializeField] Image ImageComponent;
    [SerializeField] TMP_Text ValueText;
    [SerializeField] GameObject DescriptionTooltip;
    [SerializeField] TMP_Text TooltipText;

    private void Awake()
    {
        ProvinceEvents.GetInstance().KPIUpdated += KPIUpdated;
    }
    
    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().KPIUpdated -= KPIUpdated;
    }

    private void OnDrawGizmos()
    {
        if(ImageComponent != null)
            ImageComponent.sprite = Logo;
    }

    private void Start()
    {
        ImageComponent.sprite = Logo;
        DescriptionTooltip.SetActive(false);
        TooltipText.text = TooltipValue;
    }

    void KPIUpdated(KPIUpdatedInfo info)
    {
        if(info.KPIName != KPIName)
            return;

        string valueToAddToTooltip = string.Empty;

        switch (info.Type)
        {
            case KPIType.Text:
            default:
                ValueText.text = info.ValueText;
                valueToAddToTooltip = info.ValueText;
            break;

            case KPIType.Percentage:
                ValueText.text = info.ValueNumber + " %";
                valueToAddToTooltip = ValueText.text;
            break;

            case KPIType.Number:
                ValueText.text = FormatNumber(info.ValueNumber);
                valueToAddToTooltip = info.ValueNumber.ToString("N0");
            break;
        }

        TooltipText.text = TooltipValue + " " + valueToAddToTooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionTooltip.SetActive(false);
    }

    string FormatNumber(double number)
    {
        if(number >= 1000000)
            return (number / 1000000).ToString("N0") + " M";
        else if(number >= 100000)
            return (number / 100000).ToString("N0") + " K";
        return number.ToString("N0");
    }
}
