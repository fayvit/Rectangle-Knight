using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarapaceTargetEnemy : EnemyBasic
{
    private const float forcaDeRepulsa = 850;
    private const float tempoNaRepulsao = .15f;

    protected override void Update()
    {
        FlipDirection.Flip(transform, transform.position.x - MovePoints[MoveTarget].position.x);
        base.Update();
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
