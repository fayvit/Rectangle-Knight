using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderEnemy : BaseMoveRigidbody
{
    [SerializeField] private float tempoTelegrafando = 0.5f;
    [SerializeField] private float forcaDeRepulsa = 400;
    [SerializeField] private float forDesl = 400;
    [SerializeField] private float tempoNaRepulsao = .5f;

    private EstadoDaqui estado = EstadoDaqui.movendo;

    private enum EstadoDaqui
    {
        movendo,
        atacando,
        esperandoMove,
        telegrafando
    }

    protected override void Start()
    {
        EventAgregator.AddListener(EventKey.animationPointCheck, CheckAnimationPoint);
        Invoke("TestadorDePosicao", 2);
        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.animationPointCheck, CheckAnimationPoint);
        base.OnDestroy();
    }

    void CheckAnimationPoint(IGameEvent e)
    {
        if (e.Sender == gameObject)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;
            string info = (string)ssge.MyObject[1];
            switch (info)
            {
                case "a":
                case "b":
                case "c":
                    Mov.ApplyForce(forDesl * Mathf.Sign(-transform.localScale.x) * Vector3.right,1);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.EnemySlash));
                break;
                case "d":
                    TempoDecorrido = 0;
                    _Animator.SetTrigger("retornarAoPadrao");
                    estado = EstadoDaqui.movendo;
                break;
            }

        }
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
                if (TempoDecorrido > tempoTelegrafando)
                {
                    Mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
                    estado = EstadoDaqui.atacando;
                    TempoDecorrido = 0;
                    _Animator.SetTrigger("espada");
                }
            break;
            case EstadoDaqui.esperandoMove:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
            break;
        }

        PositionChangeWithAndador();
    }

    protected override void OnActionRequest()
    {
        _Animator.SetTrigger("telegrafar");
        estado = EstadoDaqui.telegrafando;
        TempoDecorrido = 0;
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMove;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 posHeroi = GameController.g.Manager.transform.position;

        if (collision.gameObject.name != "colisorDeAtaqueComum" 
            || (estado != EstadoDaqui.movendo && estado!= EstadoDaqui.esperandoMove)
            ||(collision.gameObject.name == "colisorDeAtaqueComum" 
                && (Mathf.Sign(posHeroi.x - transform.position.x)== Mathf.Sign(transform.localScale.x))))
            base.OnTriggerEnter2D(collision);
        else if (Mathf.Sign(posHeroi.x - transform.position.x) != Mathf.Sign(transform.localScale.x))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestCharRepulse,forcaDeRepulsa*Mathf.Sign(-transform.localScale.x)*Vector3.right,tempoNaRepulsao));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
        }


    }

    protected override void OnReceivedDamageAmount(IGameEvent obj)
    {
        base.OnReceivedDamageAmount(obj);

        if (obj.Sender==gameObject && Dados.PontosDeVida > 0 && (estado==EstadoDaqui.movendo||estado==EstadoDaqui.esperandoMove))
        {
            Mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
            OnActionRequest();
        }
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
}
