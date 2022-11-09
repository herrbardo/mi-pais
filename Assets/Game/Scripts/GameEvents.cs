using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TurnChangedDelegate(int turn);

public class GameEvents
{
    private static GameEvents instance;

    private GameEvents(){}

    public static GameEvents GetInstance()
    {
        if(instance == null)
            instance = new GameEvents();
        return instance;
    }

    public event TurnChangedDelegate TurnChanged;

    public void OnTurnChanged(int turn)
    {
        if(TurnChanged != null)
            TurnChanged(turn);
    }
}
