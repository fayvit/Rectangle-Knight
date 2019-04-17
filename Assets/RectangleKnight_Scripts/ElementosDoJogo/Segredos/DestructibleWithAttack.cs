using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWithAttack : ActiveFalseForShift
{
    #region Inspector
    [SerializeField] private GameObject particuladaAcao = default;
    [SerializeField] private GameObject containerDasImagens =  default;
    [SerializeField] private Collider2D thisCollider = default;
    [SerializeField] private SoundEffectID somDaDestruicao = SoundEffectID.pedrasQuebrando;
    #endregion


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            Destruicao();
        }
    }

    public void Destruicao()
    {
        thisCollider.enabled = false;
        containerDasImagens.SetActive(false);
        particuladaAcao.SetActive(true);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDaDestruicao));
        }, 0.2f);
        Destroy(gameObject, 5);
    }

}
