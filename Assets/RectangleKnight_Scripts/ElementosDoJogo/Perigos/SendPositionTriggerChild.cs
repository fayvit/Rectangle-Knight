using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPositionTriggerChild : MonoBehaviour
{
    [SerializeField] private bool fixedPosition = false;
    [SerializeField] private Vector3 pos;

    private void Start()
    {
        pos = transform.position;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!fixedPosition)
                pos = transform.position;

            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeTeleportPosition,pos));
        }
    }
}
