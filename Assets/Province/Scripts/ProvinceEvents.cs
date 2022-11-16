using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ProvinceSelectedDelegate(ProvinceController provinceController);
public delegate void ActivityAssignedDelegate(Activity activity);
public delegate void ActivityUnassignedDelegate(Activity activity);

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

    public event ProvinceSelectedDelegate ProvinceSelected;
    public event ActivityAssignedDelegate ActivityAssigned;
    public event ActivityUnassignedDelegate ActivityUnassigned;

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

    public void OnActivityUnassigned(Activity activity)
    {
        if(ActivityUnassigned != null)
            ActivityUnassigned(activity);
    }
}
