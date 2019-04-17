using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilInimigo : MovimentaProjetil
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.heroDamage, sentidoPositivo, 25));
        }

        //Debug.Log(collision.tag+" : "+collision.name);

        if (collision.tag != "Enemy"  && collision.tag != "triggerGeral" && collision.tag!="attackCollisor")
        {
            InstanciaLigando.Instantiate(Particle, transform.position, 2);
            Destroy(gameObject);
        }

        
    }
}
