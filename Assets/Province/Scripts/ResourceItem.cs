using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ResourceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public NaturalResource ResourceInfo;
    [SerializeField] Vector3 OffsetForCard;
    [NonSerialized] public ProvinceController ProvinceController;
    
    [Header("Components")]
    [SerializeField] Image Logo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        int resourceUse = 0;
        int cooldown = 0;

        if(ProvinceController != null)
        {
            resourceUse = ProvinceController.GetResourceUse(ResourceInfo.Code);
            cooldown = ProvinceController.GetCooldown(ResourceInfo.Code);
        }

        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, true, OffsetForCard, resourceUse, cooldown);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, false, OffsetForCard, 0, 0);
    }

    private void Start()
    {
        Logo.sprite = ResourceInfo.Logo;
    }
}
