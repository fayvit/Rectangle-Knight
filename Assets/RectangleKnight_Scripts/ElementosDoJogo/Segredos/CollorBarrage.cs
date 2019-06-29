using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollorBarrage : DestructibleWithAttack
{
    [SerializeField] private SwordColor sColor = SwordColor.grey;


    private const float forcaDeRepulsa = 850;
    private const float tempoNaRepulsao = .15f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            if (GameController.g.Manager.CorDaEspadaselecionada == (int)sColor)
            {
                base.OnTriggerEnter2D(collision);
            }
            else if(collision.name!= "MagicAttack")
            {
                /*
                float difPosX = GameController.g.Manager.transform.position.x - transform.position.x;
                Vector3 dir = forcaDeRepulsa * Mathf.Sign(difPosX) * Vector3.right;

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestCharRepulse, dir , tempoNaRepulsao));

                new MyInvokeMethod().InvokeNoTempoDeJogo(() => {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
                },0.2f);*/
                SoundOnAttack.SoundAnimationAndRepulse(transform, forcaDeRepulsa, tempoNaRepulsao,collision.transform.position);

            }
        }
        
    }
}

public enum SwordColor
{
    grey,
    blue,
    green,
    gold,
    red,
    pink
}