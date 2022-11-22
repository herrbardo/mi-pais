using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    Good,
    Warning,
    Error
}

public delegate void MessagePublishedDelegate(MessageInfo messageInfo);

public class MessagesEvents
{
    private static MessagesEvents instance;

    private MessagesEvents(){ }

    public static MessagesEvents GetInstance()
    {
        if(instance == null)
            instance = new MessagesEvents();
        return instance;
    }

    public event MessagePublishedDelegate MessagePublished;

    public void OnMessagePublished(MessageInfo messageInfo)
    {
        if(MessagePublished != null)
            MessagePublished(messageInfo);
    }
}
