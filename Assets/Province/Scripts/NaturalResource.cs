using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NaturalResourceCode
{
    Woods,
    Deposit,
    Fishes,
    NaturalPlace,
    Land,
    Mountains
}

[CreateAssetMenu(fileName = "NaturalResource", menuName ="ScriptableObjects/NaturalResource")]
[Serializable]
public class NaturalResource : ScriptableObject
{
    public string Name;
    public int DurationInTurns;
    public NaturalResourceCode Code;
    public Sprite Logo;
    public bool IsBig;

    public NaturalResource(){}
}
