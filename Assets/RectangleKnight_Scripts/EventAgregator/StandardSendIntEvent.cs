using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandardSendIntEvent : IGameEvent {

    [System.NonSerialized]private GameObject _sender;
    private EventKey _key;
    private int _myInt;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key
    {
        get { return _key; }
    }

    public int MyInt
    {
        get { return _myInt; }
        private set { _myInt = value; }
    }

    public StandardSendIntEvent(GameObject sender, int myInt,EventKey key)
    {
        _sender = sender;
        _key = key;
        MyInt = myInt;
    }
}
