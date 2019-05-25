using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerStayInfo : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.triggerInfo,collision));
            }
        }
    }
}
