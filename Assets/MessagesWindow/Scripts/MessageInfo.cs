using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageInfo
{
    public MessageType Type { get; set; }
    public string Message;
    public DateTime MessageDate { get; set; }

    public MessageInfo(){}

    public MessageInfo(MessageType type, string message)
    {
        Type = type;
        Message = message;
    }
}
