using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityItem : MonoBehaviour
{
    [SerializeField] public Activity ActivityInfo;

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
}
