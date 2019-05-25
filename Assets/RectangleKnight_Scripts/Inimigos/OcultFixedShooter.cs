using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcultFixedShooter : EnemyBase
{
    [SerializeField] private float distanciaDeAparecer = 7;
    [SerializeField] private float proximoDeMais = 2;
    [SerializeField] private float intervaloDeTiro = 1;
    [SerializeField] private float velDoProjetil = 9;
    [SerializeField] private GameObject projetil = default;
    [SerializeField] private GameObject arbustinhoEsconderijo = default;
    [SerializeField] private GameObject particulaShowHide = default;
    [SerializeField] private GameObject particulaTelegrafista = default;
    [SerializeField] private Collider2D meuCollider = default;

    private EstadoDaqui estado = EstadoDaqui.emEspera;
    private Transform doHeroi;
    private float tempoDecorrido = 0;

    private enum EstadoDaqui
    {
        emEspera,
        preparandoTiro,
        emTiro
    }

    protected override void Start()
    {
        new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
        {
            doHeroi = GameController.g.Manager.transform;
        });
        
        base.Start();

        Invoke("VerificadorDeDistancia", 1);
    }

    void VerificadorDeDistancia()
    {
        if (Vector2.Distance(transform.position, doHeroi.position) < distanciaDeAparecer &&
            Vector2.Distance(transform.position, doHeroi.position) > proximoDeMais
            )
        {
            if(estado==EstadoDaqui.emEspera)
                ShowHideEnemy(true);

            estado = EstadoDaqui.preparandoTiro;
            tempoDecorrido = 0;
        }
        else
        {
            Invoke("VerificadorDeDistancia", 1);
        }
    }

    void ShowHideEnemy(bool show)
    {
        InstanciaLigando.Instantiate(particulaShowHide, transform.position, 5);
        meuCollider.enabled = show;
        arbustinhoEsconderijo.SetActive(!show);
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.preparandoTiro:
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Rotation2D.GetRotation(transform.position, doHeroi.position), 10 * Time.deltaTime);

                tempoDecorrido += Time.deltaTime;

                if (tempoDecorrido > intervaloDeTiro)
                {
                    estado = EstadoDaqui.emTiro;

                    GameObject G =  InstanciaLigando.Instantiate(projetil, transform.position, 10);
                    G.AddComponent<ProjetilQuicavel>().Iniciar(transform.right,particulaTelegrafista,velDoProjetil);

                    InstanciaLigando.Instantiate(particulaTelegrafista, transform.position, 5);
                    VerificadorDeDistancia();
                }
                else if (Vector2.Distance(transform.position, doHeroi.position) > distanciaDeAparecer ||
                        Vector2.Distance(transform.position, doHeroi.position) < proximoDeMais)
                {
                    estado = EstadoDaqui.emEspera;
                    ShowHideEnemy(false);
                    VerificadorDeDistancia();
                }
            break;
        }
    }
}
