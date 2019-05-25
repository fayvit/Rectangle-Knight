using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeParticulas : MonoBehaviour
{
    [SerializeField] GameObject particula = default;
    [SerializeField] SoundEffectID s = SoundEffectID.meuArbusto;
    [SerializeField] private float intervaloEntreSpawnParticulas = 3;

    private bool podeSpawnarParticula = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (podeSpawnarParticula && collision.tag=="Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                podeSpawnarParticula = false;
                Destroy(Instantiate(particula, collision.transform.position, Quaternion.identity), 5);

                new MyInvokeMethod().InvokeNoTempoDeJogo(() => { podeSpawnarParticula = true; }, intervaloEntreSpawnParticulas);

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, s, .5f));
            }
        }
    }
}
