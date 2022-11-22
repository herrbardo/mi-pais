using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageInfo
{
    public MessageType Type { get; set; }
    public string Message;

    public MessageInfo(){}

    public MessageInfo(MessageType type, string message)
    {
        Type = type;
        Message = message;
    }
}
