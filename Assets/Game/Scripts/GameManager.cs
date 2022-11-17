using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int Turn;
    [SerializeField] public int MaxTurn;
    [SerializeField] List<ProvinceController> Provinces;
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
        CheckForGameState();
    }

    void CheckForGameState()
    {
        if(Turn == MaxTurn)
        {
            TransitionEvents.GetInstance().OnTransitionToScene("GameFinished");
            return;
        }

        int provincesOut = Provinces.Where(p => p.State != ProvinceState.Working).Count();
        if(provincesOut == Provinces.Count)
        {
            TransitionEvents.GetInstance().OnTransitionToScene("GameFinished");
            return;
        }
    }
}
