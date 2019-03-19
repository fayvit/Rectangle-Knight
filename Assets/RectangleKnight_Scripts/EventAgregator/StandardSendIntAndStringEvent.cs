using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandardSendIntAndStringEvent : IGameEvent
{
    [System.NonSerialized]private GameObject _sender;
    private EventKey _key;
    private int _myInt;
    private string _myString;

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

    public string MyString
    {
        get { return _myString; }
        private set { _myString = value; }
    }

    public StandardSendIntAndStringEvent(GameObject sender, int myInt, string myString,EventKey key)
    {
        _sender = sender;
        _key = key;
        MyInt = myInt;
        MyString = myString;
    }
}
