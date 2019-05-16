using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoQueTeleportaQuicavel : DanoQueTelePorta
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "attackCollisor")
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.colisorNoQuicavel,collision.name));
            SoundOnAttack.SoundAndAnimation(transform, collision.transform.position);
        }
    }
}
