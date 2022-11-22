using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessagesWindow : MonoBehaviour
{
    [SerializeField] GameObject MessageLinePrefab;
    [SerializeField] GameObject Container;
    [SerializeField] float PostMessageInterval;

    int currentTurn;

    Queue<MessageInfo> messages;

    private void Awake()
    {
        messages = new Queue<MessageInfo>();
        MessagesEvents.GetInstance().MessagePublished += MessagePublished;
        GameEvents.GetInstance().TurnChanged += TurnChanged;
    }

    private void Start()
    {
        InvokeRepeating("PostMessage", 1f, PostMessageInterval);
        currentTurn = 1;
    }

    private void OnDestroy()
    {
        MessagesEvents.GetInstance().MessagePublished -= MessagePublished;
        GameEvents.GetInstance().TurnChanged -= TurnChanged;
    }

    void MessagePublished(MessageInfo messageInfo)
    {
        messages.Enqueue(messageInfo);
        
    }

    void PostMessage()
    {
        if(messages == null || messages.Count == 0)
            return;

        MessageInfo info = messages.Dequeue();
        GameObject newLine = Instantiate(MessageLinePrefab);
        newLine.transform.parent = Container.transform;
        newLine.transform.localScale = new Vector3(1f, 1f, 1f);
        newLine.transform.SetSiblingIndex(0);

        TMP_Text text = newLine.GetComponent<TMP_Text>();
        text.text = string.Format("[T:{0}] - {1}", currentTurn, info.Message);
        switch (info.Type)
        {
            case MessageType.Good:
            default:
                text.color = Color.green;
            break;

            case MessageType.Warning:
                text.color = Color.yellow;
            break;

            case MessageType.Error:
                text.color = Color.red;
            break;
        }

        RectTransform rect = Container.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public bool AnyMessagesLeft()
    {
        return messages.Count > 0;
    }

    void TurnChanged(int turn)
    {
        this.currentTurn = turn;
    }
}
