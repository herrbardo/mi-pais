using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPIUpdatedInfo
{
    public KPIName KPIName { get; set; }
    public double Value { get; set; }

    public KPIUpdatedInfo(KPIName kpiName, double value)
    {
        KPIName = kpiName;
        Value = value;
    }
}
