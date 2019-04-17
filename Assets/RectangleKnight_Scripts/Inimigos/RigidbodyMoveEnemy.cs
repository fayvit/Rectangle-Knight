using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RigidbodyMoveEnemy : BaseMoveRigidbody
{
    
    [SerializeField] private float INVEST_SPEED = 12;
    [SerializeField] private float TEMPO_ACELERANDO = 0.25f;

    
    public float InvestSpeed { get => INVEST_SPEED;}
    
    
    public Vector3 DirDaAceleracao { get; set; } = Vector3.zero;

    protected abstract void OnFinallyAccelerate();    
    protected abstract void OnActionActivate();

    protected override void Start()
    {
        
        PreviousMoveTarget = transform.position;
        PosAnterior = transform.position;
        
        
        base.Start();
    }

    

    protected void UpdateAcelerando()
    {
        TempoDecorrido += Time.deltaTime;
        if (TempoDecorrido < TEMPO_ACELERANDO)
        {
            Mov.AplicadorDeMovimentos(
            Vector3.ProjectOnPlane(DirDaAceleracao, Vector3.up));
        }
        else
        {
            TempoDecorrido = 0;
            _Animator.SetTrigger("retornarAoPadrao");
            OnFinallyAccelerate();//estado = EstadoDaqui.finalizandoInvestida;
            Mov.ChangeSpeed(StandardSpeed);
        }
    }

    protected void Acelerar()
    {
        _Animator.SetTrigger("avancar");
        UltimaAcelerada = Time.time;
        TempoDecorrido = 0;
        DirDaAceleracao = Vector3.ProjectOnPlane(GameController.g.Manager.transform.position - transform.position, Vector3.up).normalized;
        Mov.ChangeSpeed(InvestSpeed);
        OnActionActivate();//estado = EstadoDaqui.investindo;
    }

    


}