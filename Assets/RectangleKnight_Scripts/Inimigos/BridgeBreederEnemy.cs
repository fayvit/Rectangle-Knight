using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBreederEnemy : BreederBase
{
    [SerializeField] private float tempoDeSpawn = 1;
    [SerializeField] private float distanciaParaAtiar=25;
    private const float forcaDeRepulsa = 850;
    private const float tempoNaRepulsao = .15f;

    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("VerifiqueSpawn",tempoDeSpawn);
        base.Start();
    }

    void VerifiqueSpawn()
    {
        float dist = Vector2.Distance(transform.position, GameController.g.Manager.transform.position);
        Debug.Log(dist);
        if (dist<distanciaParaAtiar)
            Telegrafar(transform.position);

        Invoke("VerifiqueSpawn", tempoDeSpawn);
    }

    protected override void OnDefeated()
    {
        EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.triggerInfo));
        base.OnDefeated();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, Dados.AtaqueBasico));
            }
        }

        if (collision.tag == "attackCollisor")
        {
            if (collision.name == "MagicAttack")
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage, collision.name));
            else if (collision.name == "colisorDoAtaquebaixo")
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.colisorNoQuicavel, collision.name));
                SoundOnAttack.SoundAndAnimation(transform, collision.transform.position);
            }
            else
                SoundOnAttack.SoundAnimationAndRepulse(transform, forcaDeRepulsa, tempoNaRepulsao, collision.transform.position);
        }
    }
}
