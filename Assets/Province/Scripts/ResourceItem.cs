using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public NaturalResource ResourceInfo;
    [SerializeField] Vector3 OffsetForCard;
    
    [Header("Components")]
    [SerializeField] Image Logo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, true, OffsetForCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, false,OffsetForCard);
    }

    private void Start()
    {
        Logo.sprite = ResourceInfo.Logo;
    }
}
