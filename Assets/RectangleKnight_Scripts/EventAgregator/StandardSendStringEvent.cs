using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandardSendStringEvent : IGameEvent {

    [System.NonSerialized] private GameObject _sender;

    private string _nomeParticulasGolpe;    
    private EventKey _key;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key
    {
        get { return _key; }
    }

    public string StringContent
    {
        get { return _nomeParticulasGolpe; }
        private set { _nomeParticulasGolpe = value; }
    }

    public StandardSendStringEvent(GameObject sender,string nomeGolpe,EventKey key)
    {
        _nomeParticulasGolpe = nomeGolpe;
        _sender = sender;
        _key = key;
    }
}
