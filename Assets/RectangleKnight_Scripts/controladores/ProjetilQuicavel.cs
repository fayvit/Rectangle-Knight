using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilQuicavel : MovimentaProjetil
{ 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, 25));
        }

        //Debug.Log(collision.tag + " : " + collision.name);

        if (collision.tag != "Enemy" && collision.tag != "triggerGeral" && collision.tag != "attackCollisor" && collision.gameObject.layer != 12)
        {
            InstanciaLigando.Instantiate(Particle, transform.position, 2);
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.request3dSound, SoundEffectID.Break, 1f));
            Destroy(gameObject);
        }

        if (collision.tag == "attackCollisor")
        {
            gameObject.tag = "attackCollisor";
            
            Vector3 plusDir = Vector3.zero;

            switch (collision.gameObject.name)
            {
                case "colisorDeAtaqueComum":
                    plusDir = Mathf.Sign(GameController.g.Manager.transform.localScale.x)*Vector2.right;
                break;
                case "colisorDoAtaquebaixo":
                    plusDir = Vector3.down;
                break;
                case "colisorDoAtaqueAlto":
                    plusDir = Vector3.up;
                break;
            }

            Dir = -Dir + plusDir;

            if (Dir != Vector3.zero)
                Dir = Dir.normalized;
            else
                Dir = plusDir;

            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,() =>{
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.request3dSound, SoundEffectID.Break, 1f));
            },.15f);
        }


    }
}
