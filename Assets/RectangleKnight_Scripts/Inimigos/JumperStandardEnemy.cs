using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperStandardEnemy : RigidbodyMoveEnemy
{
    [SerializeField] private GameObject particulaTelegrafista = default;
    private EstadoDaqui estado = EstadoDaqui.movendo;

    [SerializeField] private float TEMPO_TELEGRAFANDO_PULO = 0.25F;
    private enum EstadoDaqui
    {
        movendo,
        esperandoMovimento,
        telegrafandoPulo,
        pulando,
        tempoDepoisDoPulo
    }


    protected override void OnActionActivate()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnActionRequest()
    {
        GameObject G = Instantiate(particulaTelegrafista, particulaTelegrafista.transform.position, Quaternion.identity);
        G.SetActive(true);
        Destroy(G, 1);
        TempoDecorrido = 0;
        estado = EstadoDaqui.telegrafandoPulo;
    }

    protected override void OnFinallyAccelerate()//finallyJump
    {
        throw new System.NotImplementedException();
    }

    protected override void OnTargetCheck()
    {
        estado = EstadoDaqui.esperandoMovimento;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("TestadorDePosicao", 2);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.movendo:
                UpdateMovendo();
            break;
            case EstadoDaqui.esperandoMovimento:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TempoEsperando)
                    estado = EstadoDaqui.movendo;
            break;
            case EstadoDaqui.telegrafandoPulo:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_TELEGRAFANDO_PULO)
                {
                    DirDaAceleracao = Vector3.ProjectOnPlane(GameController.g.Manager.transform.position - transform.position, Vector3.up).normalized;
                    UltimaAcelerada = Time.time;
                    Mov.ChangeSpeed(InvestSpeed);
                    Mov.Pulo();
                    estado = EstadoDaqui.pulando;
                    TempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.pulando:
                Mov.AplicadorDeMovimentos(DirDaAceleracao);

                Debug.Log(Mov.NoChao);
                TempoDecorrido += Time.deltaTime;

                if (Mov.NoChao && TempoDecorrido>0.5f)
                {
                    
                    TempoDecorrido = 0;
                    estado = EstadoDaqui.tempoDepoisDoPulo;
                }
            break;
            case EstadoDaqui.tempoDepoisDoPulo:
                TempoDecorrido += Time.deltaTime;
                Mov.AplicadorDeMovimentos(Vector3.zero);
                if (TempoDecorrido > TempoEsperando)
                {
                    Mov.ChangeSpeed(StandardSpeed);
                    estado = EstadoDaqui.movendo;
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
}
