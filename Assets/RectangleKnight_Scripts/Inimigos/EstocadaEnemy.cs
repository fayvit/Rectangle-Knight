using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstocadaEnemy : RigidbodyMoveEnemy
{
    [SerializeField] private float TEMPO_TELEGRAFANDO = 1;

    private EstadoDaqui estado = EstadoDaqui.movendo;
    
    private enum EstadoDaqui
    {
        movendo,
        preparando,
        investindo,
        esperandoMove,
        finalizando
    }

    protected override void Start()
    {
        Invoke("TestadorDePosicao", 2);
        base.Start();
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


    private void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.movendo:
                UpdateMovendo();
            break;
            case EstadoDaqui.esperandoMove:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
            break;
            case EstadoDaqui.preparando:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_TELEGRAFANDO)
                {
                    Acelerar();
                    estado = EstadoDaqui.investindo;
                }
            break;
            case EstadoDaqui.investindo:
                UpdateAcelerando();
            break;
            case EstadoDaqui.finalizando:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
            break;
        }

        PositionChangeWithAndador();
    }

    protected override void OnActionActivate()
    {
        //OnActionRequest();
        //throw new System.NotImplementedException();
    }

    protected override void OnActionRequest()
    {
        _Animator.SetTrigger("telegrafar");
        estado = EstadoDaqui.preparando;
        TempoDecorrido = 0;
    }

    protected override void OnFinallyAccelerate()
    {
        estado = EstadoDaqui.finalizando;
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMove;
    }
}