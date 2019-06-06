using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetaSombriaTrigger : AtivadorDoBotaoConversa
{
    [SerializeField] private BossSetaSombria containerDoBoss = default;
    [SerializeField] private GameObject bloqueioPorFazer = default;
    [SerializeField] private GameObject particulaDoBloqueio = default;
    [SerializeField] private RestritorDeCamLimits restritor = default;

    /*
    new protected void Start()
    {
        base.Start();
    }

    new protected void Update()
    {
        base.Update();
    }*/

    protected override void OnStartTalk()
    {
        restritor.MudeLimitantesParaTrigger(0.25f);
    }

    protected override void OnFinishTalk()
    {
        bloqueioPorFazer.SetActive(true);
        InstanciaLigando.Instantiate(particulaDoBloqueio, bloqueioPorFazer.transform.position, 5);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom,SoundEffectID.pedrasQuebrando));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.returnRememberedMusic));
        containerDoBoss.IniciaApresentacaoDoBoss(transform.position);
        gameObject.SetActive(false);
    }
}
