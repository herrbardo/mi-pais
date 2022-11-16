using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public NaturalResource ResourceInfo;
    
    [Header("Components")]
    [SerializeField] Image Logo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseResourceAction(ResourceInfo, false);
    }

    private void Start()
    {
        Logo.sprite = ResourceInfo.Logo;
    }
}
