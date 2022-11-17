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
        int resourceUse = -1;
        if(ProvinceController != null)
            resourceUse = ProvinceController.GetResourceUse(ResourceInfo.Code);

        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, true, OffsetForCard, resourceUse);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, false, OffsetForCard, -1);
    }

    private void Start()
    {
        Logo.sprite = ResourceInfo.Logo;
    }
}
