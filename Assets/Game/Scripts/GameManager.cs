using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] MessagesWindow MessagesWindow;

    [Header("Data")]
    [SerializeField] public int Turn;
    [SerializeField] public int MaxTurn;
    [SerializeField] List<ProvinceController> Provinces;

    [NonSerialized] public DateTime CurrentDate;

    private void Awake()
    {
        CurrentDate = new DateTime(DateTime.Now.Year, 1, 1);
    }

    private void Start()
    {
        MessagesEvents.GetInstance().OnMessagePublished(new MessageInfo(MessageType.Good, "¡Buen día Presidente!"));
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
            StartCoroutine(CheckingMessagesAndFinallyLeave());
            return;
        }

        int provincesOut = Provinces.Where(p => p.State != ProvinceState.Working).Count();
        if(provincesOut == Provinces.Count)
        {
            StartCoroutine(CheckingMessagesAndFinallyLeave());
            return;
        }
    }

    IEnumerator CheckingMessagesAndFinallyLeave()
    {
        while(MessagesWindow.AnyMessagesLeft())
            yield return new WaitForSeconds(5);

        TransitionEvents.GetInstance().OnTransitionToScene("GameFinished");
        yield return null;
    }
}
