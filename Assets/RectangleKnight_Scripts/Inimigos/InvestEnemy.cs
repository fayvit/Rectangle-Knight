using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestEnemy : RigidbodyMoveEnemy
{
    
    [SerializeField] private GameObject particulaDaInvestida = default;

    [SerializeField] private float TEMPO_DA_PAUSA_DA_INVESTIDA = 0.25F;

    private EstadoDaqui estado = EstadoDaqui.movendo;

    private enum EstadoDaqui
    {
        movendo,
        preparandoInvestida,
        investindo,
        esperandoMove,
        finalizandoInvestida
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("TestadorDePosicao", 2);
        base.Start();
    }



    protected override void OnActionRequest()
    {
        GameObject G = Instantiate(particulaDaInvestida, particulaDaInvestida.transform.position, Quaternion.identity) as GameObject;
        G.SetActive(true);
        Destroy(G, .5f);
        TempoDecorrido = 0;
        estado = EstadoDaqui.preparandoInvestida;
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

    protected override void OnFinallyAccelerate()
    {
        estado = EstadoDaqui.finalizandoInvestida;
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMove;
    }

    protected override void OnActionActivate()
    {
        OnActionRequest();
    }
}
