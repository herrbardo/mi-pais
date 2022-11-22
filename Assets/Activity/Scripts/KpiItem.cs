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
        // if(DescriptionTooltip != null)
        //     DescriptionTooltip.SetActive(false);
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

        ValueText.text = info.Value.ToString("N0");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionTooltip.SetActive(false);
    }
}
