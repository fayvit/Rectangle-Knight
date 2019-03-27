using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RigidbodyMoveEnemy : TargedEnemy
{
    [SerializeField] private Transform andador;
    [SerializeField] private MovimentacaoBasica mov;

    private Vector3 posAnterior = Vector3.zero;
    
    [SerializeField] private float DISTANCIA_DE_ACELERACAO = 7;
    [SerializeField] private float STANDARD_SPEED = 3;
    [SerializeField] private float INVEST_SPEED = 12;
    [SerializeField] private float TEMPO_ACELERANDO = 0.25f;
    [SerializeField] private float TEMPO_ESPERANDO = 0.5F;
    [SerializeField] private float INTERVALO_ENTRE_INVESTIDAS = 2.5f;

    public float TempoDecorrido { get; set; } = 0;
    public float TempoEsperando { get => TEMPO_ESPERANDO; set => TEMPO_ESPERANDO = value; }
    public MovimentacaoBasica Mov { get => mov; }
    public float InvestSpeed { get => INVEST_SPEED;}
    public float UltimaAcelerada { get; set; } = 0;
    public float StandardSpeed { get => STANDARD_SPEED; set => STANDARD_SPEED = value; }
    public Vector3 DirDaAceleracao { get; set; } = Vector3.zero;

    protected abstract void OnFinallyAccelerate();
    protected abstract void OnTargetCheck();
    protected abstract void OnActionRequest();
    protected abstract void OnActionActivate();

    protected override void Start()
    {
        mov.Iniciar(andador);
        mov.ChangeSpeed(StandardSpeed);
        PreviousMoveTarget = transform.position;
        posAnterior = transform.position;
        UltimaAcelerada = -INTERVALO_ENTRE_INVESTIDAS;
        
        base.Start();
    }

    protected void UpdateMovendo()
    {
        mov.AplicadorDeMovimentos(
                    Vector3.ProjectOnPlane(MovePoints[MoveTarget].position - transform.position, Vector3.up).normalized);

        if (Mathf.Abs(MovePoints[MoveTarget].position.x - transform.position.x) < 0.1f)
        {
            TempoDecorrido = 0;
            OnTargetCheck();//estado = EstadoDaqui.esperandoMove;
            TrocaMoveTarget();
        }

        if (Mathf.Abs(GameController.g.Manager.transform.position.x - transform.position.x) < DISTANCIA_DE_ACELERACAO
            &&
            Time.time - UltimaAcelerada > INTERVALO_ENTRE_INVESTIDAS
            )
        {
            OnActionRequest();//PrepararInvestida();
        }
    }

    protected void UpdateAcelerando()
    {
        TempoDecorrido += Time.deltaTime;
        if (TempoDecorrido < TEMPO_ACELERANDO)
        {
            mov.AplicadorDeMovimentos(
            Vector3.ProjectOnPlane(DirDaAceleracao, Vector3.up));
        }
        else
        {
            TempoDecorrido = 0;
            _Animator.SetTrigger("retornarAoPadrao");
            OnFinallyAccelerate();//estado = EstadoDaqui.finalizandoInvestida;
            mov.ChangeSpeed(StandardSpeed);
        }
    }

    protected void Acelerar()
    {
        _Animator.SetTrigger("avancar");
        UltimaAcelerada = Time.time;
        TempoDecorrido = 0;
        DirDaAceleracao = Vector3.ProjectOnPlane(GameController.g.Manager.transform.position - transform.position, Vector3.up).normalized;
        mov.ChangeSpeed(InvestSpeed);
        OnActionActivate();//estado = EstadoDaqui.investindo;


    }

    protected void TestadorDePosicaoBase()
    {
        if (Mathf.Abs(transform.position.x - posAnterior.x) == 0)
        {
            mov.Pulo();
        }

        posAnterior = transform.position;
    }

    protected void PositionChangeWithAndador()
    {
        transform.position = andador.position;
        Vector3 t = transform.localScale;
        transform.localScale = new Vector3(-1 * Mathf.Abs(t.x) * Mathf.Sign(andador.localScale.x), t.y, t.z);
    }
}