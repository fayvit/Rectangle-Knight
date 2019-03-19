using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandardGameEvent : IGameEvent
{
    [System.NonSerialized]private GameObject _sender;
    private EventKey _key;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key{
        get {return _key;}
    }

    public StandardGameEvent(GameObject sender,EventKey key)
    {
        _sender = sender;
        _key = key;
    }
}
