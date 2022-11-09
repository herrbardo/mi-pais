using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ActivityContainer : MonoBehaviour, IDropHandler
{
    [SerializeField] List<ActivityItem> ActivitiesToShow;

    
    private void Awake()
    {
        DisableAllActivities();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ActivityItem item = eventData.pointerDrag.gameObject.GetComponent<ActivityItem>();
        Activity activity = item.ActivityInfo;
        ProvinceEvents.GetInstance().OnActivityAssigned(activity);
    }

    public void DisableAllActivities()
    {
        foreach (ActivityItem item in ActivitiesToShow)
            item.gameObject.SetActive(false);
    }

    public void EnableActivity(string name)
    {
        ActivityItem item = ActivitiesToShow.Where(a => a.ActivityInfo.Name.Equals(name)).First();
        item.gameObject.SetActive(true);
    }
}
