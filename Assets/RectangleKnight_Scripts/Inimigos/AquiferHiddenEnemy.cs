using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquiferHiddenEnemy : EnemyBasic
{
    [SerializeField] private Transform posDoSalto = default;
    [SerializeField] private Transform retornoAoChao = default;
    [SerializeField] private GameObject particula = default;
    [SerializeField] private float zAngleTarget = 0;
    [SerializeField] private SoundEffectID s = SoundEffectID.meuArbusto;
    

    private float TempoDecorrido = 0;
    private float TEMPO_TELEGRAFANDO_PULO = 0.75f;
    private float TEMPO_SUBINDO = 0.25f;
    private float TEMPO_DESCENDO = 0.15F;
    private Quaternion rotOriginal;
    private Vector3 posOriginal;
    private EstadoDaqui estado = EstadoDaqui.escondido;

    private enum EstadoDaqui
    {
        escondido,
        aparecendo,
        disparaPulo,
        descendoPulo,
        baseUpdate
    }

    protected override void Start()
    {
        posOriginal = transform.position;
        base.Start();
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if (ssge.Sender.transform.IsChildOf(transform.parent) && estado == EstadoDaqui.escondido)
        {
            if (((Collider2D)ssge.MyObject[0]).tag == "Player")
            {
                estado = EstadoDaqui.aparecendo;
                Destroy(Instantiate(particula,transform.position, Quaternion.identity), 5);

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, s, .5f));
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.aparecendo:
                TempoDecorrido += Time.deltaTime;

                Vector3 alvo = posOriginal + 0.15f * Vector3.right;
                Vector3 partida = posOriginal;
                float tempo = 10 * TempoDecorrido / TEMPO_TELEGRAFANDO_PULO - ((int)(10 * TempoDecorrido / TEMPO_TELEGRAFANDO_PULO));
                if (((int)(10 * TempoDecorrido / TEMPO_TELEGRAFANDO_PULO) )% 2 == 0)
                {
                    alvo = posOriginal;
                    partida = posOriginal+0.15f * Vector3.right;
                }

                transform.position = Vector3.Lerp(partida,alvo,tempo);
                if (TempoDecorrido > TEMPO_TELEGRAFANDO_PULO)
                {
                    TempoDecorrido = 0;
                    rotOriginal = transform.rotation;
                    estado = EstadoDaqui.disparaPulo;
                }
            break;
            case EstadoDaqui.disparaPulo:
                TempoDecorrido += Time.deltaTime;
                Quaternion qAlvo = Quaternion.Euler(0, 0, zAngleTarget);
                if (TempoDecorrido < TEMPO_SUBINDO)
                {
                    transform.position = Vector3.Lerp(posOriginal, posDoSalto.position, ZeroOneInterpolation.PolinomialInterpolation(TempoDecorrido / TEMPO_SUBINDO, 2));
                    transform.rotation = Quaternion.Lerp(rotOriginal, qAlvo, TempoDecorrido / TEMPO_SUBINDO);
                }
                else
                {
                    PreviousMoveTarget = retornoAoChao.position;
                    transform.rotation = qAlvo;
                    TempoDecorrido = 0;
                    estado = EstadoDaqui.descendoPulo;
                }
            break;
            case EstadoDaqui.descendoPulo:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido < TEMPO_DESCENDO)
                {
                    transform.position = Vector3.Lerp(posDoSalto.position, retornoAoChao.position, ZeroOneInterpolation.PolinomialInterpolation(TempoDecorrido / TEMPO_DESCENDO, 2));
                }
                else
                {
                    transform.position = retornoAoChao.position;
                    estado = EstadoDaqui.baseUpdate;
                }
            break;
            case EstadoDaqui.baseUpdate:
                FlipDirection.Flip(transform,zAngleTarget==0 
                    ? PreviousMoveTarget.x-MovePoints[MoveTarget].position.x
                    : Mathf.Sign(zAngleTarget)*(PreviousMoveTarget.y - MovePoints[MoveTarget].position.y));
                base.Update();
            break;
        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(estado!= EstadoDaqui.escondido && estado!=EstadoDaqui.aparecendo)
            base.OnTriggerEnter2D(collision);
    }

}
