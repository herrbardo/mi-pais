using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MouseActivityActionDelegate(Activity activity, bool enter);
public delegate void MouseResourceActionDelegate(NaturalResource resource, bool enter);

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
    public event MouseResourceActionDelegate MouseResourceAction;

    public void OnMouseActivityAction(Activity item, bool enter)
    {
        if(MouseActivityAction != null)
            MouseActivityAction(item, enter);
    }

    public void OnMouseResourceAction(NaturalResource resource, bool enter)
    {
        if(MouseResourceAction != null)
            MouseResourceAction(resource, enter);
    }
}
