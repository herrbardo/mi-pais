using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActivityItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Activity ActivityInfo;
    [SerializeField] Vector3 OffsetForCard;

    Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        RefreshLogo();
    }

    public void RefreshLogo()
    {
        if(ActivityInfo != null)
            imageComponent.sprite = ActivityInfo.Logo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseActivityAction(this.ActivityInfo, true, OffsetForCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.GetInstance().OnMouseActivityAction(this.ActivityInfo, false, OffsetForCard);
    }
}
