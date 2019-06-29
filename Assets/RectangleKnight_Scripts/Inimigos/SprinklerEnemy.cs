using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerEnemy : BaseMoveRigidbody
{
    [SerializeField] private GameObject borrifadaFinal = default;
    [SerializeField] private GameObject particulaDoBorrifar = default;
    [SerializeField] private Collider2D colisorDeDano = default;
    [SerializeField] private float TEMPO_TELEGRAFANDO = .5F;
    [SerializeField] private float TEMPO_DO_BORRIFAR_AO_DANO = .15F;

    private EstadoDaqui estado = EstadoDaqui.movendo;

    private enum EstadoDaqui
    {
        movendo,
        borrifando,
        esperandoMove,
        telegrafando,
        emEspera
    }

    protected override void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTrigerInfo);
        Invoke("TestadorDePosicao", 2);
        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTrigerInfo);
        base.OnDestroy();
    }

    void OnReceivedTrigerInfo(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if (ssge.MyObject.Length > 0)
            if (ssge.MyObject[0] is Collider2D)
            {
                Collider2D collision = (Collider2D)ssge.MyObject[0];

                if (ssge.Sender.transform.IsChildOf(transform) && collision.tag == "Player" && colisorDeDano.enabled)
                {
                    OnTriggerEnter2D(collision);
                }
            }

    }

    protected override void OnActionRequest()
    {
        EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, SoundEffectID.Fire1));
        UltimaAcelerada = Time.time;
        _Animator.SetTrigger("telegrafar");
        estado = EstadoDaqui.telegrafando;
        TempoDecorrido = 0;
    }

    protected override void OnTargetCheck()
    {
        _Animator.SetTrigger("retornarAoPadrao");
        estado = EstadoDaqui.esperandoMove;
    }

    void Update()
    {

        switch (estado)
        {
            case EstadoDaqui.movendo:
                UpdateMovendo();
            break;
            case EstadoDaqui.telegrafando:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_TELEGRAFANDO)
                {
                    Mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
                    estado = EstadoDaqui.borrifando;
                    InstanciaLigando.Instantiate(particulaDoBorrifar, transform.position, 5);
                    EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, SoundEffectID.Darkness4));
                    TempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.esperandoMove:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                {
                    _Animator.SetTrigger("anda");
                    estado = EstadoDaqui.movendo;
                }
            break;
            case EstadoDaqui.borrifando:
                TempoDecorrido += Time.deltaTime;
                if(TempoDecorrido>TEMPO_DO_BORRIFAR_AO_DANO)
                {
                    colisorDeDano.enabled = true;

                    estado = EstadoDaqui.emEspera;
                    new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
                    {
                        if (this != null)
                        {
                            _Animator.SetTrigger("retornarAoPadrao");
                            colisorDeDano.enabled = false;
                            estado = EstadoDaqui.movendo;
                        }
                    }, .5f);
                }

            break;
        }

        PositionChangeWithAndador();

    }

    private void TestadorDePosicao()
    {
        switch (estado)
        {
            case EstadoDaqui.movendo:
                TestadorDePosicaoBase();
            break;
        }

        Invoke("TestadorDePosicao", 2);
    }

    protected override void OnDefeated()
    {
        borrifadaFinal.transform.position = transform.position;
        borrifadaFinal.SetActive(true);
        Vector3 V = transform.position - GameController.g.Manager.transform.position + Vector3.up;
        V.Normalize();
        borrifadaFinal.GetComponent<Rigidbody2D>().AddForce(300*V);
        base.OnDefeated();
    }
}
