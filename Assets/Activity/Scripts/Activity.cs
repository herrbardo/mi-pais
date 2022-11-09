using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "Activity", menuName ="ScriptableObjects/Activity")]
[Serializable]
public class Activity : ScriptableObject
{
    public string Name;
    public Sprite Logo;
    public int IncomePerPerson;
    public int AmountEmployees;
    public int EnvironmentImpactPerTurn;
    public List<NaturalResourceCode> CompatibleResources;

    public Activity(){}
}
