using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerInfo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.triggerInfo, collision));
    }
}
