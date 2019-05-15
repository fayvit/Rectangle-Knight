using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquiferAccelerateEnemy : RigidbodyMoveEnemy
{
    [SerializeField] private float TEMPO_DA_PAUSA_DA_INVESTIDA = 0.25f;
    private EstadoDaqui estado = EstadoDaqui.movendo;

    private enum EstadoDaqui
    {
        movendo,
        preparandoInvestida,
        investindo,
        esperandoMove,
        finalizandoInvestida
    }

    protected override void OnFinallyAccelerate()
    {
        Mov.AplicadorDeMovimentos(0.25f*(Mov.Velocity.normalized));
        estado = EstadoDaqui.finalizandoInvestida;
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMove;
    }

    protected override void OnActionActivate()
    {
        //OnActionRequest();
    }

 

    protected override void OnActionRequest()
    {
        _Animator.SetTrigger("preparar");
        TempoDecorrido = 0;
        Mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
        Mov.AplicadorDeMovimentos(Vector3.zero);
        estado = EstadoDaqui.preparandoInvestida;
        
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("TestadorDePosicao", 2);
        base.Start();
    }

    void Update()
    {
        //Debug.Log(estado+" : "+ (MovePoints[MoveTarget].position - transform.position)+" : "+ (Vector3.ProjectOnPlane((MovePoints[MoveTarget].position - transform.position).normalized, Vector3.up)));

        switch (estado)
        {
            case EstadoDaqui.movendo:
                UpdateMovendo();
            break;
            case EstadoDaqui.preparandoInvestida:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_DA_PAUSA_DA_INVESTIDA)
                {
                    Acelerar();
                    estado = EstadoDaqui.investindo;
                }
            break;
            case EstadoDaqui.investindo:
                UpdateAcelerando();
            break;
            case EstadoDaqui.esperandoMove:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
            break;
            case EstadoDaqui.finalizandoInvestida:
                TempoDecorrido += Time.deltaTime;

                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
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
}
