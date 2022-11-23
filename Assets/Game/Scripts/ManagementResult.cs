using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementResult
{
    private static ManagementResult instance;

    private ManagementResult(){}

    public static ManagementResult GetInstance()
    {
        if(instance == null)
            instance = new ManagementResult();
        return instance;
    }

    public List<ProvinceController> Provinces{ get; set; }
}
