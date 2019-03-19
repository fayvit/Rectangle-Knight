using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSendGameEvent : IGameEvent
{
    public GameObject Sender { get; set; }

    public EventKey Key { get; set; }

    public object[] MyObject { get; set; }

    public StandardSendGameEvent(GameObject sender,EventKey key, params object[] o)
    {
        Sender = sender;
        Key = key;
        MyObject = o;
    }

    public StandardSendGameEvent(EventKey key, params object[] o)
    {
        Sender = null;
        Key = key;
        MyObject = o;
    }
}
