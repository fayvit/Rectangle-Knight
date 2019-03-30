using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSender : MonoBehaviour
{
    private DadosDoPersonagem dados;

    private void Start()
    {
        dados = transform.parent.GetComponent<CharacterBase>().Dados; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision.name + " : " + collision.tag);
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, dados.AtaqueBasico));
            }
        }


        if (collision.tag == "attackCollisor" && gameObject.layer == 11)
        {
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage, collision.name));
        }
    }
}
