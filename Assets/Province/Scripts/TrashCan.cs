using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    public bool ShowTrashCan;

    private void Awake()
    {
        UIEvents.GetInstance().ElementBeginDrag += ElementBeginDrag;
        UIEvents.GetInstance().ElementEndDrag += ElementEndDrag;
    }

    private void Start()
    {
        Hide();
    }

    private void OnDestroy()
    {
        UIEvents.GetInstance().ElementBeginDrag -= ElementBeginDrag;
        UIEvents.GetInstance().ElementEndDrag -= ElementEndDrag;
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }

    void Show()
    {
        if(!ShowTrashCan)
            return;
        this.gameObject.SetActive(true);
    }

    public void OnDrop(PointerEventData eventData)
    {
        ActivityItem item = eventData.pointerDrag.gameObject.GetComponent<ActivityItem>();
        Activity activity = item.ActivityInfo;
        ProvinceEvents.GetInstance().OnActivityUnassigned(activity);
        Hide();
    }

    void ElementBeginDrag(PointerEventData eventData)
    {
        Show();
    }

    void ElementEndDrag(PointerEventData eventData)
    {
        Hide();
    }
}
