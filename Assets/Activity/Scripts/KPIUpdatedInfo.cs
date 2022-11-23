using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KPIType
{
    Number,
    Percentage,
    Text
}

public class KPIUpdatedInfo
{
    public KPIName KPIName { get; set; }
    public double ValueNumber { get; set; }
    public string ValueText { get; set; }
    public KPIType Type { get; set; }

    public KPIUpdatedInfo(KPIName kpiName, double value, KPIType type)
    {
        KPIName = kpiName;
        ValueNumber = value;
        Type = type;
    }

    public KPIUpdatedInfo(KPIName kpiName, string value)
    {
        KPIName = kpiName;
        ValueText = value;
        Type = KPIType.Text;
    }

    public KPIUpdatedInfo(KPIName kpiName, double value): this(kpiName,value, KPIType.Number){}
}
