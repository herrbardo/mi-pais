using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Province", menuName ="ScriptableObjects/ProvinceInfo")]
[Serializable]
public class ProvinceInfo : ScriptableObject
{
    public string DisplayName;
    public List<NaturalResources> NaturalResources;

    public ProvinceInfo(){}
}
