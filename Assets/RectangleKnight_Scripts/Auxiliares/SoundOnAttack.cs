using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnAttack : MonoBehaviour
{
    private const float forcaDeRepulsa = 850;
    private const float tempoNaRepulsao = .15f;

    public static void SoundAnimationAndRepulse(Transform transform,float forcaDeRepulsa,float tempoNaRepulsao,Vector3 pos)
    {
        float difPosX = GameController.g.Manager.transform.position.x - transform.position.x;
        Vector3 dir = forcaDeRepulsa * Mathf.Sign(difPosX) * Vector3.right;

        
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestCharRepulse, dir, tempoNaRepulsao));
        SoundAndAnimation(transform,pos);
        
    }

    public static void SoundAndAnimation(Transform transform,Vector3 pos)
    {
        InstanciaLigando.Instantiate((GameObject)Resources.Load("impactAnimation"), 0.5f * (transform.position + pos), 1.15f);
        new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
        }, 0.2f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "attackCollisor" && collision.gameObject.name != "magicAttack")
        {
            SoundAnimationAndRepulse(transform, forcaDeRepulsa, tempoNaRepulsao,collision.transform.position);
        }
    }
}
