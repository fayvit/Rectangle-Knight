using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultBiterEnemy : NonRespawnOnLoadEnemy
{
    [SerializeField] private GameObject particulaDoInicio=default;
    [SerializeField] private float tempoTelegrafando = 0.25f;
    [SerializeField] private float tempoPosMordida = 1.25f;
    [SerializeField] private float intervaloEntreMordidas = 2;
    [SerializeField] private float intervaloDeDano = 1;

    private bool emDano = false;
    private float ultimaMordida = 0;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        telegrafando,
        mordendo,
        voltando
    }

    protected override void Start()
    {
        ultimaMordida = Time.deltaTime - intervaloEntreMordidas;
        _Animator = GetComponent<Animator>();
        EventAgregator.AddListener(EventKey.animationPointCheck, OnAnimationPointReceived);
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.animationPointCheck, OnAnimationPointReceived);
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.OnDestroy();
    }

    void OnAnimationPointReceived(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        if (e.Sender == gameObject)
        {
            estado = EstadoDaqui.emEspera;
        }
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        //Debug.Log(ssge.Sender.name+" : "+estado);
        if (ssge.Sender.transform.IsChildOf(transform))
        {
            Collider2D c = (Collider2D)ssge.MyObject[0];
            
            if (ssge.Sender.name == "triggerAcionador" && estado==EstadoDaqui.emEspera && Time.time-ultimaMordida>intervaloEntreMordidas)
            {
                estado = EstadoDaqui.telegrafando;
                InstanciaLigando.Instantiate(particulaDoInicio, transform.position, 5);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Fire1));
                _Animator.SetTrigger("telegrafar");
                new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,PosTelegrafar,tempoTelegrafando);
            }
            else if (ssge.Sender.name == "mordedorEscondido")
            {
                VerificadorDeDano(c);
            }
        }
    }

    void VerificadorDeDano(Collider2D c)
    {
        if ((estado == EstadoDaqui.mordendo || estado == EstadoDaqui.voltando) && !emDano && c.gameObject.tag == "Player")
        {
            
            emDano = true;
            OnTriggerEnter2D(c);
            new MyInvokeMethod().InvokeNoTempoDeJogo(() => { emDano = false; }, intervaloDeDano);
        }
        else if (c.gameObject.tag == "attackCollisor" && estado!=EstadoDaqui.emEspera)
        {
            Debug.Log(estado);
            OnTriggerEnter2D(c);
        }
    }

    void PosTelegrafar()
    {
        _Animator.SetTrigger("morder");
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.mordida));
        estado = EstadoDaqui.mordendo;
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PosMorder, tempoPosMordida);
    }

    void PosMorder()
    {
        ultimaMordida = Time.time;
        _Animator.SetTrigger("voltar");
        estado = EstadoDaqui.voltando;
    }
}
