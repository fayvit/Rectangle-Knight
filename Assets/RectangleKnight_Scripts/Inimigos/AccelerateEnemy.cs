using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateEnemy : RigidbodyMoveEnemy
{
    private EstadoDaqui estado = EstadoDaqui.movendo;

    private enum EstadoDaqui
    { 
        movendo,
        acelerando, 
        esperandoMove
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("TestadorDePosicao",2);
        base.Start();
    }

  

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(estado+" : "+ (MovePoints[MoveTarget].position - transform.position)+" : "+ (Vector3.ProjectOnPlane((MovePoints[MoveTarget].position - transform.position).normalized, Vector3.up)));

        switch (estado)
        {
            case EstadoDaqui.movendo:
                UpdateMovendo();
            break;
            case EstadoDaqui.acelerando:
                UpdateAcelerando();
            break;
            case EstadoDaqui.esperandoMove:
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

    protected override void OnFinallyAccelerate()
    {
        estado = EstadoDaqui.movendo;
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMove;
    }

    protected override void OnActionRequest()
    {
        Acelerar();
    }

    protected override void OnActionActivate()
    {
        estado = EstadoDaqui.acelerando;
    }
}
