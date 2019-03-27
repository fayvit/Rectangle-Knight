using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMethodEvent : IGameEvent
{
    public object[] MyObject { get; }
    public System.Action Acao { get; }
    public GameObject Sender { get; }

    public EventKey Key { get; }

    public SendMethodEvent(EventKey key, System.Action acao, params object[] o)
    {

        Key = key;
        Acao += acao;
        MyObject = o;

    }
}
