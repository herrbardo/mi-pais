using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MouseActivityActionDelegate(Activity activity, bool enter);

public class UIEvents
{
    private static UIEvents instance;

    private UIEvents(){}

    public static UIEvents GetInstance()
    {
        if(instance == null)
            instance = new UIEvents();
        return instance;
    }

    public event MouseActivityActionDelegate MouseActivityAction;

    public void OnMouseActivityAction(Activity item, bool enter)
    {
        if(MouseActivityAction != null)
            MouseActivityAction(item, enter);
    }
}
