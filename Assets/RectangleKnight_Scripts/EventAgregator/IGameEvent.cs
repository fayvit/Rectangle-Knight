using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    GameObject Sender{get;}
    EventKey Key { get; }
}