using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ProvinceLost", menuName ="ScriptableObjects/ProvinceLostInfo")]
[Serializable]
public class ProvinceLostInfo : ScriptableObject
{
    public ProvinceState State;
    public Sprite Logo;
    public string Description;

    public ProvinceLostInfo(){}
}
