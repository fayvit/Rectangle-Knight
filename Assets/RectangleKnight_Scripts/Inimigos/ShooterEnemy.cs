using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : StrategyMovementEnemy
{
    #region inspector
    [SerializeField] private GameObject particulaTelegrafista = default;
    [SerializeField] private GameObject projetil = default;
    #endregion

    protected override void Telegrafar(Vector3 charPos)
    {
        FlipDirection.Flip(transform, charPos.x - transform.position.x);

        InstanciaLigando.Instantiate(particulaTelegrafista, transform.position, 5);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Wind1));
    }

    

    protected override void RequestAction(Vector3 charPos)
    {
        Vector3 dir = charPos - transform.position;
        GameObject G = InstanciaLigando.Instantiate(projetil, transform.position, 5,
            Quaternion.LookRotation(projetil.transform.forward,
            Vector3.Cross(dir, projetil.transform.forward)));

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));

        ProjetilInimigo P = G.AddComponent<ProjetilInimigo>();
        P.Iniciar(dir, particulaTelegrafista, 10f);
        P.SomDeImpacto = SoundEffectID.lancaProjetilInimigo;

        RetornarParaEsperaZerandoTempo();

        
    }
}
