using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int Turn;
    [SerializeField] public int MaxTurn;
    public DateTime CurrentDate;

    private void Awake()
    {
        CurrentDate = new DateTime(DateTime.Now.Year, 1, 1);
    }

    public void MoveNextTurn()
    {
        Turn++;
        CurrentDate = CurrentDate.AddMonths(1);
        GameEvents.GetInstance().OnTurnChanged(Turn);
    }
}
