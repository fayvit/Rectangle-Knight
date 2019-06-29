using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoSetaSombria : MonoBehaviour
{
    //[SerializeField] private GameObject particulasPreparaAnimaMorte = default;
    [SerializeField] private GameObject avatar = default;
    [SerializeField] private GameObject particulaPosSumico = default;
    [SerializeField] private GameObject[] barreiras = default;
    [SerializeField] private AudioClip gritoDoDanoFatal = default;
    [SerializeField] private AudioClip dasParticulasAnimaMorte = default;
    [SerializeField] private AudioClip dasParticulasFimDoAnimaMorte = default;
    [SerializeField] private SpriteFinalizadorDeBoss spriteFinBoss = default;
    [SerializeField] private NPCdeConversa npc=default;
    [SerializeField] private PainelUmaMensagem pMensagem=default;
    [SerializeField] private PainelUmaMensagem pMensagemDois = default;

    [SerializeField] private float tempoPosSumico = 0.75f;
    [SerializeField] private float tempoDePainelPosSumico = 2;



    private BossSetaSombria boss;
    private Vector3 bossPosition;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        spriteFinalizador,
        falaFinal,
        visualizandoParticula

    }
    public void IniciarFinalDoSetaSombria(BossSetaSombria boss)
    {
        this.boss = boss;
        bossPosition = boss.transform.position;
        EventAgregator.Publish(EventKey.abriuPainelSuspenso);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, gritoDoDanoFatal));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic));
        spriteFinBoss.InstanciarSpriteFinalizador(bossPosition);
        estado = EstadoDaqui.spriteFinalizador;
    }

    private void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.spriteFinalizador:
                if (spriteFinBoss.Update())
                {
                    boss.InvocaTeleportProps(false);
                    new MyInvokeMethod().InvokeNoTempoDeJogo(AparecendoDoTeleport,.75f);
                    estado = EstadoDaqui.emEspera;
                }
            break;
            case EstadoDaqui.falaFinal:
                if (npc.Update())
                {
                    boss.InvocaTeleportProps(false);
                    avatar.SetActive(false);
                    new MyInvokeMethod().InvokeNoTempoDeJogo(ParticulaPosSumico,tempoPosSumico);
                    EventAgregator.Publish(EventKey.abriuPainelSuspenso);
                    estado = EstadoDaqui.emEspera;
                }
            break;
        }
    }

    void ParticulaPosSumico()
    {
        estado = EstadoDaqui.visualizandoParticula;
        particulaPosSumico.SetActive(true);
        AudioDoAnimaMorte();
        new MyInvokeMethod().InvokeNoTempoDeJogo(PainelPosSumico,tempoDePainelPosSumico);
        
    }

    void AudioDoAnimaMorte()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, dasParticulasAnimaMorte));

        if (estado == EstadoDaqui.visualizandoParticula)
        {
            Invoke("AudioDoAnimaMorte", 0.25f);
        }
    }

    void PainelPosSumico()
    {
        estado = EstadoDaqui.emEspera;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, dasParticulasFimDoAnimaMorte));
        pMensagem.ConstroiPainelUmaMensagem(PainelPosSumicoDois);
    }

    void PainelPosSumicoDois()
    {
        estado = EstadoDaqui.emEspera;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.ItemImportante));
        pMensagemDois.ConstroiPainelUmaMensagem(RetornoDoPainel);
    }


    void RetornoDoPainel()
    {
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        EventAgregator.Publish(EventKey.getMagicAttack);
        EventAgregator.Publish(EventKey.requestSceneCamLimits);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, KeyShift.venceuSetaSombria));
    

        for (int i = 0; i < barreiras.Length; i++)
        {
            barreiras[i].GetComponent<CollorBarrage>().Destruicao();
        }
    }

    void AparecendoDoTeleport()
    {
        boss.transform.position = avatar.transform.position;
        boss.InvocaTeleportProps(false);
        avatar.gameObject.SetActive(true);
        npc.Start(transform);
        npc.IniciaConversa();
        estado = EstadoDaqui.falaFinal;
    }
}
