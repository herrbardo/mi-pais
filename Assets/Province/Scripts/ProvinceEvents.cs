using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ProvinceSelectedDelegate(ProvinceController provinceController);
public delegate void ActivityAssignedDelegate(Activity activity);

public class ProvinceEvents
{
    private static ProvinceEvents instance;

    private ProvinceEvents(){ }

    public static ProvinceEvents GetInstance()
    {
        if(instance == null)
            instance = new ProvinceEvents();
        return instance;
    }

    public ProvinceSelectedDelegate ProvinceSelected;
    public ActivityAssignedDelegate ActivityAssigned;

    public void OnProvinceSelected(ProvinceController provinceController)
    {
        if(ProvinceSelected!= null)
            ProvinceSelected(provinceController);
    }

    public void OnActivityAssigned(Activity activity)
    {
        if(ActivityAssigned != null)
            ActivityAssigned(activity);
    }
}
