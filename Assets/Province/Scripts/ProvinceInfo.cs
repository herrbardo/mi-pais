using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Province", menuName ="ScriptableObjects/ProvinceInfo")]
[Serializable]
public class ProvinceInfo : ScriptableObject
{
    public string DisplayName;
    public List<NaturalResource> NaturalResources;
    public int Population;
    public int Money;
    public int MonthlyExpenses;
    public int EnvironmentPoints;

    public ProvinceInfo(){}
}
