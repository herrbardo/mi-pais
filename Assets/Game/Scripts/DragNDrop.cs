using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas parentCanvas;
    RectTransform rectTransform;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] HorizontalLayoutGroup LayoutGroup;
    
    [Header("Behaviour")]
    [SerializeField] bool ReturnToOriginWhenRelease;

    Vector3 originalLocalPosition;

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        UIEvents.GetInstance().OnElementBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        UIEvents.GetInstance().OnElementEndDrag(eventData);
        if(ReturnToOriginWhenRelease)
        {
            transform.localPosition = originalLocalPosition;
            if(LayoutGroup != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(LayoutGroup.GetComponent<RectTransform>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
