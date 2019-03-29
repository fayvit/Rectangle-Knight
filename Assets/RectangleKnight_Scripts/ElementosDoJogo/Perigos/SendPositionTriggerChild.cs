using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPositionTriggerChild : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EventAgregator.Publish(new StandardGameEvent(gameObject, EventKey.changeTeleportPosition));
        }
    }
}
