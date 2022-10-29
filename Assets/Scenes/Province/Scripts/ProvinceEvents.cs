using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ProvinceSelectedDelegate(ProvinceInfo info);

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

    public void OnProvinceSelected(ProvinceInfo info)
    {
        if(ProvinceSelected!= null)
            ProvinceSelected(info);
    }
}
