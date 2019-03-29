using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoQueTelePorta : MonoBehaviour
{
    private Vector3 teleportPosition;
    // Start is called before the first frame update
    void Start()
    {
        teleportPosition = transform.GetChild(0).position;
        EventAgregator.AddListener(EventKey.changeTeleportPosition,OnChangeTeleportPositionRequest);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.changeTeleportPosition, OnChangeTeleportPositionRequest);
    }

    void OnChangeTeleportPositionRequest(IGameEvent e)
    {
        if (e.Sender.transform.IsChildOf(transform))
        {
            teleportPosition = e.Sender.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo,25,teleportPosition));
            }
        }
    }
}
