using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquiferWarrior : NonRespawnOnLoadEnemy
{
    [SerializeField] private float tempoTelegrafandoEspada = 0.5f;
    [SerializeField] private float tempoTelegrafandoProjetil = .5f;
    [SerializeField] private float tempoDeBackDash = 1;
    [SerializeField] private float forcaDeRepulsa = 400;
    [SerializeField] private float forDesl = 400;
    [SerializeField] private float tempoNaRepulsao = .5f;
    [SerializeField] private float coolDown = 1.5f;
    [SerializeField] private float intervalosDeMagias = .75f;
    [SerializeField] private float distanciaDeEspadada = 2;
    [SerializeField] private float distanciaDeDesligar = 10;

    [SerializeField] private GameObject particulaTelegrafista=default;
    [SerializeField] private GameObject particulaDeAtaque=default;
    [SerializeField] private MovimentacaoBasica mov=default;
    [SerializeField] private Transform andador=default;

    private float tempoDecorrido = 0;
    private Vector3 posOriginal;
    private Transform doHeroi;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        emAtaque,
        aproximandoSe,
        backdash,
        buscadorDeAcao,
        defendendoAcima,
        retorneAoInicio
    }

    protected override void Start()
    {
        mov.Iniciar(andador);
        posOriginal = transform.position;
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceiveTriggerInfo);
        EventAgregator.AddListener(EventKey.animationPointCheck, OnReceivedAnimationPoint);
        base.Start();

        new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
        {
            mov.AplicadorDeMovimentos(
                        DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
        });
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceiveTriggerInfo);
        EventAgregator.RemoveListener(EventKey.animationPointCheck, OnReceivedAnimationPoint);
        base.OnDestroy();
    }

    private void OnReceiveTriggerInfo(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if (ssge.Sender.transform.IsChildOf(transform) && estado == EstadoDaqui.emEspera)
        {
            if (((Collider2D)ssge.MyObject[0]).tag == "Player")
            {
                doHeroi = GameController.g.Manager.transform;
                estado = EstadoDaqui.buscadorDeAcao;
                /*
                
                
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMusicWithRecovery, new NameMusicaComVolumeConfig()
                {
                    Musica = NameMusic.miniBoss,
                    Volume = 1
                }));*/
            }
        }
    }

    void PosTelegrafarEspada()
    {
        _Animator.SetTrigger("espadadaUm");
        mov.ApplyForce(forDesl * Mathf.Sign(-transform.localScale.x) * Vector3.right, 1);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.EnemySlash));
    }

    void OnReceivedAnimationPoint(IGameEvent e)
    {
        if (e.Sender == gameObject)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;
            string info = (string)ssge.MyObject[1];
            switch (info)
            {
                case "a":
                    
                break;
                case "b":
                    int random = Random.Range(0, 3);
                    if (random < 2)
                    {
                        _Animator.SetTrigger("retornaAoPadrao");
                        estado = EstadoDaqui.buscadorDeAcao;
                        tempoDecorrido = 0;
                    }
                    else {
                        _Animator.SetTrigger("espadadaDois");
                    }
                break;
                case "c":
                    _Animator.SetTrigger("retornaAoPadrao");
                    estado = EstadoDaqui.buscadorDeAcao;
                    tempoDecorrido = 0;
                break;
                case "e":
                    mov.ApplyForce(forDesl * Mathf.Sign(-transform.localScale.x) * Vector3.right, 1);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.EnemySlash));
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.aproximandoSe:
                mov.AplicadorDeMovimentos(
                    DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));

                if (Mathf.Abs(doHeroi.position.x - transform.position.x) < 0.9f * distanciaDeEspadada)
                {
                    estado = EstadoDaqui.buscadorDeAcao;
                }
            break;
            case EstadoDaqui.buscadorDeAcao:
                tempoDecorrido += Time.deltaTime;

                //FlipDirection.Flip(andador.transform,-transform.position.x + doHeroi.transform.position.x);

                if (doHeroi.position.y > transform.position.y)
                {
                    estado = EstadoDaqui.defendendoAcima;
                    _Animator.SetTrigger("defendendoAcima");
                }
                else
                if (tempoDecorrido > coolDown)
                {
                    //mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, doHeroi.position));
                    FlipDirection.Flip(andador.transform, transform.position.x - doHeroi.transform.position.x);
                    if (Mathf.Abs(doHeroi.position.x - transform.position.x) < distanciaDeEspadada)
                    {
                        int random = Random.Range(0, 10);

                        if (random <= 5)
                        {
                            IniciaAtaque();
                        }
                        else
                        {

                            if (BoaDistanciaAtras())
                            {
                                FlipDirection.Flip(andador.transform, -transform.position.x + doHeroi.transform.position.x);
                                estado = EstadoDaqui.backdash;
                                mov.AplicadorDeMovimentos(
                                    DirecaoNoPlano.NoUpNormalizado(GameController.g.Manager.transform.position, transform.position), true);
                                tempoDecorrido = 0;
                            }
                            else
                            {
                                IniciaAtaque();
                            }
                        }
                    }
                    else
                    {
                        int sorteio = Random.Range(0, 5);

                        if (sorteio <= 1)
                        {
                            estado = EstadoDaqui.aproximandoSe;
                        }
                        else
                        {
                            IniciaProjetil();
                        }
                    }
                }
                else if (Vector2.Distance(transform.position, doHeroi.position) > distanciaDeDesligar)
                {
                    estado = EstadoDaqui.retorneAoInicio;
                }
                
            break;
            case EstadoDaqui.retorneAoInicio:
                if (Vector2.Distance(doHeroi.position, transform.position) > distanciaDeDesligar)
                {
                    if (Vector2.Distance(transform.position, posOriginal) < 0.5f)
                    {
                        EventAgregator.Publish(EventKey.returnRememberedMusic, null);
                        estado = EstadoDaqui.emEspera;
                    }
                    else
                        mov.AplicadorDeMovimentos(DirecaoNoPlano.NoUpNormalizado(transform.position, posOriginal));
                }
                else
                {
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.buscadorDeAcao;
                }
            break;
            case EstadoDaqui.defendendoAcima:
                if (doHeroi.position.y <= transform.position.y)
                {
                    _Animator.SetTrigger("retornoDoDefendendoAcima");
                    estado = EstadoDaqui.buscadorDeAcao;
                }
            break;
            case EstadoDaqui.backdash:
                FlipDirection.Flip(andador.transform,transform.position.x - doHeroi.transform.position.x);
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido < tempoDeBackDash)
                {
                    mov.AplicadorDeMovimentos(
                    DirecaoNoPlano.NoUpNormalizado(GameController.g.Manager.transform.position, transform.position));
                }
                else
                {
                    mov.AplicadorDeMovimentos(
                    DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
                    FlipDirection.Flip(andador.transform, transform.position.x - doHeroi.transform.position.x);
                    if (Mathf.Abs(doHeroi.position.x - transform.position.x) > 1.1f * distanciaDeEspadada)
                        IniciaProjetil();
                    else
                    {
                        estado = EstadoDaqui.buscadorDeAcao;
                        tempoDecorrido = 0;
                    }
                }
            break;
        }

        
        BaseMoveRigidbody.PositionWithAndador(andador, transform);
    }

    void IniciaProjetil()
    {
        _Animator.SetTrigger("telegrafarProjetil");

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Wind1));
        InstanciaLigando.Instantiate(particulaTelegrafista, transform.GetChild(1).position, 5);
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PosTelegrafarProjetil, tempoTelegrafandoProjetil);
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PosProjetil, intervalosDeMagias);

        estado = EstadoDaqui.emAtaque;
    }

    void PosProjetil()
    {
        bool foi = false;
        if (Mathf.Abs(doHeroi.position.x - transform.position.x) > 1.1f * distanciaDeEspadada)
        {
            int random = Random.Range(0, 8);

            if (random > 0)
            {
                foi = true;
            }
        }

        if (foi)
        {
            IniciaProjetil();
        }
        else
        {
            estado = EstadoDaqui.buscadorDeAcao;
            tempoDecorrido = 0;
        }
    }

    void PosTelegrafarProjetil()
    {
        _Animator.SetTrigger("retornoDoProjetil");

        GameObject G = InstanciaLigando.Instantiate(particulaDeAtaque, transform.GetChild(1).position, 15);

        ProjetilParabolico P = G.AddComponent<ProjetilParabolico>();

        Vector3 posAlvo = transform.position + 0.7f * (DirecaoNoPlano.NoUp(transform.position, doHeroi.position))+2*Vector3.down;
        P.Iniciar(posAlvo,
            new Vector2(0.5f * (transform.position.x + posAlvo.x), transform.position.y + 5),particulaTelegrafista, 15
            );
    }

    void IniciaAtaque()
    {
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,PosTelegrafarEspada, tempoTelegrafandoEspada);

        _Animator.SetTrigger("telegrafarEspada");
        estado = EstadoDaqui.emAtaque;
    }

    bool BoaDistanciaAtras()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(andador.transform.position+ Mathf.Sign(-andador.localScale.x) * Vector3.right, 
            Mathf.Sign(-andador.localScale.x)*Vector3.right,1000,256);

        Debug.Log(hit.transform);
        if (hit)
            if (Mathf.Abs(hit.point.x - transform.position.x) < 7)
                return false;

        return true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 posHeroi = GameController.g.Manager.transform.position;

        if (collision.tag == "Player"
            || collision.gameObject.name == "MagicAttack"
             || estado == EstadoDaqui.emAtaque 
            || //(collision.gameObject.name == "colisorDeAtaqueComum"
                 (Mathf.Sign(posHeroi.x - transform.position.x) == Mathf.Sign(transform.localScale.x)))
            base.OnTriggerEnter2D(collision);
        else if (Mathf.Sign(posHeroi.x - transform.position.x) != Mathf.Sign(transform.localScale.x))
        {

            if (collision.gameObject.name == "colisorDoAtaquebaixo")
            {
                SoundOnAttack.SoundAndAnimation(transform, collision.transform.position);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.colisorNoQuicavel, collision.name));
            }
            else if (collision.tag == "attackCollisor")
                SoundOnAttack.SoundAnimationAndRepulse(transform, forcaDeRepulsa, tempoNaRepulsao, collision.transform.position);

           // EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestCharRepulse, forcaDeRepulsa * Mathf.Sign(-transform.localScale.x) * Vector3.right, tempoNaRepulsao));
            //EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
        }


    }
}
