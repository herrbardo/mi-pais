using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TMP_Text Date;
    [SerializeField] TMP_Text Turn;

    private void Awake()
    {
        GameEvents.GetInstance().TurnChanged += TurnChanged;
    }

    private void OnDestroy()
    {
        GameEvents.GetInstance().TurnChanged -= TurnChanged;
    }

    void Start()
    {
        DisplayTurnInfo();
    }

    void DisplayTurnInfo()
    {
        Date.text = Utilities.ConvertDateToDisplayMonthDate(GameManager.CurrentDate);
        Turn.text = string.Format("{0:00}/{1}", GameManager.Turn, GameManager.MaxTurn);
    }

    void TurnChanged(int turn)
    {
        DisplayTurnInfo();
    }
}
