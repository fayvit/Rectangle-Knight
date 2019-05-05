using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCirculoImperfeito : EnemyBase
{
    [SerializeField] private MovimentacaoBasica mov = default;
    [SerializeField] private Transform andador = default;
    [SerializeField] private Transform conjuntoDeEspinhos = default;
    [SerializeField] private Transform parabola1 = default;
    [SerializeField] private Transform parabola2 = default;
    [SerializeField] private Transform cantoAlto1 = default;
    [SerializeField] private Transform cantoAlto2 = default;
    [SerializeField] private Transform vertice = default;
    [SerializeField] private GameObject particulaDoAtaqueReto = default;
    [SerializeField] private GameObject particulaDoAtaqueAlto = default;
    [SerializeField] private GameObject particulaEnfaseDoBoss = default;
    [SerializeField] private AudioClip[] uhuhah = default;
    [SerializeField] private AudioClip urroDeAtaque = default;
    [SerializeField] private AudioClip gritoDaFuria = default;
    [SerializeField] private AudioClip preparaEspinhos = default;
    [SerializeField] private AudioClip lancaEspinhos = default;
    [SerializeField] private AudioClip gritoDoDanoFatal = default;

    private float distancia = 0;
    private float tempoDecorrido = 0;
    private int contDePulinhos = 0;
    private int indice = 0;
    private int contFurias = 0;
    private bool requisicaoDeEspinhos = false;
    private Vector3 posInicial;
    private Vector3 target;
    private Vector3[] targets;
    private EspinhosDoCirculoImperfeito[] lancados;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private const float velocidadeGiroFuria = 25;
    private const float TEMPO_ENTRE_PULINHO = 0.5F;
    private const float TEMPO_PREPARANDO_PULINHO = 0.5F;
    private const float TEMPO_NO_PULINHO = 0.75F;
    private const float TEMPO_PREPARANDO_ESPINHOS = 0.5f;
    private const float TEMPO_PARA_LANCAR_ESPINHOS = 0.5f;
    private const float TEMPO_TELEGRAFANDO_GIRO = 1;
    private const float TEMPO_GIRO_ALTO = 1.5F;
    private const float TEMPO_PREPARANDO_FURIA = 1.5F;
    private const float PULINHOS_ATE_ATAQUE = 2;

    private enum EstadoDaqui
    {
        emEspera,
        pulinhosPreparatorios,
        preparaPulinho,
        atualizaPulinho,
        preparaEspinhos,
        lancaEspinhos,
        telegrafaMovimento,
        giroReto,
        giroAlto,
        giroDeFuria,
        prepararGiroDeFuria,
        finalizado
    }

    protected override void Start()
    {
        mov.Iniciar(andador);
        _Animator = GetComponent<Animator>();
        base.Start();
    }

    protected override void OnReceivedDamageAmount(IGameEvent obj)
    {
        if (obj.Sender == gameObject)
        {
            if (
                estado == EstadoDaqui.atualizaPulinho
                || estado == EstadoDaqui.preparaPulinho
                || estado == EstadoDaqui.pulinhosPreparatorios
                )
                requisicaoDeEspinhos = true;

            base.OnReceivedDamageAmount(obj);
        }
    }

    protected override void OnDefeated()
    {
        
        Destroy(GetComponent<CapsuleCollider2D>());
        estado = EstadoDaqui.finalizado;

        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, gritoDoDanoFatal));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic));
        FindObjectOfType<FinalBossManager>().IniciarFinalizacao(transform);
    }

    void FinalizaGiro()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.pedrasQuebrando));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 10, 2f));
        transform.rotation = Quaternion.identity;
        mov.ChangeGravityScale(5);
        mov.AplicadorDeMovimentos(-DirecaoNoPlano.NoUpNormalizado(transform.position, GameController.g.Manager.transform.position));
        estado = EstadoDaqui.pulinhosPreparatorios;
        contDePulinhos = 0;
        tempoDecorrido = 0;
    }

    bool VerifiqueContFurias()
    {
        if (contFurias == 0 && Dados.PontosDeVida < 0.75f * Dados.MaxVida
        || (contFurias == 1 && Dados.PontosDeVida < 0.5f * Dados.MaxVida)
        || (contFurias == 2 && Dados.PontosDeVida < 0.25f * Dados.MaxVida)
        )
        {
            contFurias++;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (estado != EstadoDaqui.emEspera)
        {
            tempoDecorrido += Time.deltaTime;
            BaseMoveRigidbody.PositionWithAndador(andador, transform);
        }

        switch (estado)
        {
            case EstadoDaqui.pulinhosPreparatorios:
                #region pulinhosPreparatorios
                if (VerifiqueContFurias())
                {
                    _Animator.SetTrigger("queda");
                    InstanciaLigando.Instantiate(particulaEnfaseDoBoss, transform.position, 5);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, gritoDaFuria));
                    EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
                    estado = EstadoDaqui.prepararGiroDeFuria;
                }
                else
                if (tempoDecorrido > TEMPO_ENTRE_PULINHO && contDePulinhos < PULINHOS_ATE_ATAQUE)
                {
                    int qual = Random.Range(0, uhuhah.Length);

                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, uhuhah[qual]));
                    _Animator.SetTrigger("preparaPulo");
                    estado = EstadoDaqui.preparaPulinho;
                    tempoDecorrido = 0;
                }
                else if (tempoDecorrido > TEMPO_ENTRE_PULINHO && contDePulinhos >= PULINHOS_ATE_ATAQUE)
                {
                    int qual = Random.Range(0, 2);

                    if (qual == 0)
                    {
                        InstanciaLigando.Instantiate(
                            particulaDoAtaqueAlto,
                            particulaDoAtaqueAlto.transform.position, 5,
                            Quaternion.identity);

                        estado = EstadoDaqui.giroAlto;
                        PreparaParabolaLocal();
                    }
                    else if (qual == 1)
                    {
                        EscolheDestino();
                        InstanciaLigando.Instantiate(particulaDoAtaqueReto, particulaDoAtaqueReto.transform.position, 5);
                        estado = EstadoDaqui.giroReto;
                    }
                    else
                    {
                        Debug.Log("numero não esperado: " + qual);
                    }

                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, urroDeAtaque));
                    tempoDecorrido = 0;
                }
                #endregion
            break;
            case EstadoDaqui.prepararGiroDeFuria:
                #region preparandoGiroFuria
                if (tempoDecorrido > TEMPO_PREPARANDO_FURIA)
                {
                    _Animator.SetTrigger("tocouChao");
                    EventAgregator.Publish(EventKey.fechouPainelSuspenso);
                    EscolheMelhorCantoAlto();
                    estado = EstadoDaqui.giroDeFuria;
                    tempoDecorrido = 0;
                    contDePulinhos = 0;
                    indice = 0;
                    mov.ChangeGravityScale(0);
                }
                #endregion
            break;
            case EstadoDaqui.giroDeFuria:
                #region giroFuria
                andador.position = Vector3.Lerp(posInicial,targets[indice],velocidadeGiroFuria*tempoDecorrido/distancia);
                transform.Rotate(new Vector3(0, 0, 20));

                if (tempoDecorrido * velocidadeGiroFuria > distancia)
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.pedrasQuebrando));
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 5, 2f));
                    tempoDecorrido = 0;
                    posInicial = targets[indice];
                    indice = ContadorCiclico.AlteraContador(1, indice, 4);
                    contDePulinhos++;
                }

                if (contDePulinhos >= 19)
                {
                    _Animator.SetTrigger("retornaAoPadrao");
                    FinalizaGiro();
                }
                #endregion
            break;
            case EstadoDaqui.giroReto:
                #region giroReto
                if (tempoDecorrido > TEMPO_TELEGRAFANDO_GIRO + TEMPO_GIRO_ALTO)
                {

                    FinalizaGiro();
                    //mov.AplicadorDeMovimentos(Vector3.zero);
                }
                else if (tempoDecorrido > TEMPO_TELEGRAFANDO_GIRO)
                {
                    float time = (tempoDecorrido - TEMPO_TELEGRAFANDO_GIRO) / TEMPO_GIRO_ALTO;

                    transform.Rotate(new Vector3(0, 0, 20));

                    andador.position = Vector3.Lerp(posInicial, target, ZeroOneInterpolation.PolinomialInterpolation(time, 2));
                }
                #endregion
            break;
            case EstadoDaqui.giroAlto:
                #region giroAlto
                if (tempoDecorrido > TEMPO_TELEGRAFANDO_GIRO+ TEMPO_GIRO_ALTO)
                {

                    FinalizaGiro();
                    //mov.AplicadorDeMovimentos(Vector3.zero);
                }
                else if(tempoDecorrido>TEMPO_TELEGRAFANDO_GIRO)
                {
                    float time = (tempoDecorrido - TEMPO_TELEGRAFANDO_GIRO) / TEMPO_GIRO_ALTO;

                    transform.Rotate(new Vector3(0,0,20));
                    andador.position = ZeroOneInterpolation.ParabolaDeDeslocamento(
                        new Vector2(posInicial.x,posInicial.y), 
                        new Vector2(target.x,target.y), 
                        new Vector2(vertice.position.x,vertice.position.y), 
                        ZeroOneInterpolation.OddPolynomialInterpolation(time,3));

                    //Debug.Log(target.x+" : "+posInicial.x);
                }
                #endregion
            break;
            case EstadoDaqui.preparaPulinho:
                #region preparaPulinhos
                if (tempoDecorrido > TEMPO_PREPARANDO_PULINHO)
                {
                    mov.JumpForce();
                    _Animator.SetTrigger("disparaPulo");
                    tempoDecorrido = 0;
                    contDePulinhos++;
                    estado = EstadoDaqui.atualizaPulinho;
                }
                #endregion
            break;
            case EstadoDaqui.atualizaPulinho:
                #region atualizaPulinhos
                if (tempoDecorrido > TEMPO_NO_PULINHO)
                {
                    if (requisicaoDeEspinhos)
                    {
                        lancados = new EspinhosDoCirculoImperfeito[conjuntoDeEspinhos.childCount];

                        for (int i = 0; i < conjuntoDeEspinhos.childCount; i++)
                        {
                            GameObject G = InstanciaLigando.Instantiate(conjuntoDeEspinhos.GetChild(i).gameObject,
                                conjuntoDeEspinhos.GetChild(i).position, 10);

                            lancados[i] = G.GetComponent<EspinhosDoCirculoImperfeito>();
                        }

                        _Animator.SetTrigger("retornaAoPadrao");
                        _Animator.SetTrigger("queda");

                        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, preparaEspinhos));
                        estado = EstadoDaqui.preparaEspinhos;
                        requisicaoDeEspinhos = false;
                    }
                    else
                    {
                        _Animator.SetTrigger("retornaAoPadrao");
                        estado = EstadoDaqui.pulinhosPreparatorios;    
                    }

                    tempoDecorrido = 0;
                }
                #endregion
            break;
            case EstadoDaqui.preparaEspinhos:
                #region preparaEspinhos
                if (tempoDecorrido>TEMPO_PREPARANDO_ESPINHOS)
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, lancaEspinhos));
                    _Animator.SetTrigger("tocouChao");
                    mov.JumpForce();
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.lancaEspinhos;
                }
                #endregion
            break;
            case EstadoDaqui.lancaEspinhos:
                #region lancaEspinhos
                if (tempoDecorrido > TEMPO_PARA_LANCAR_ESPINHOS)
                {
                    _Animator.SetTrigger("retornaAoPadrao");

                    for (int i = 0; i < lancados.Length; i++)
                    {
                        lancados[i].circulo.SetActive(false);
                        lancados[i].espinhos.SetActive(true);
                        lancados[i].particulaDosEspinhos.SetActive(true);
                    }

                    estado = EstadoDaqui.pulinhosPreparatorios;
                    tempoDecorrido = 0;
                }
                #endregion
            break;
        }
    }

    void EscolheMelhorCantoAlto()
    {
        posInicial = transform.position;
        targets = Vector3.Distance(cantoAlto1.position, transform.position) > Vector3.Distance(cantoAlto2.position, transform.position)
            ?
             new Vector3[4] { cantoAlto2.position, cantoAlto1.position, parabola1.position, parabola2.position }
            :
            new Vector3[4] { cantoAlto1.position, cantoAlto2.position, parabola2.position, parabola1.position };

        distancia = Vector3.Distance(posInicial,targets[0]);
    }

    void EscolheDestino()
    {
        posInicial = transform.position;
        target = Vector3.Distance(transform.position, parabola1.position) > Vector3.Distance(transform.position, parabola2.position)
            ?
            parabola1.position
            :
            parabola2.position;
    }

    void PreparaParabolaLocal()
    {

        EscolheDestino();
        mov.ChangeGravityScale(0);
    }

    public void IniciarBoss()
    {
        
        andador.position = transform.position;
        estado = EstadoDaqui.pulinhosPreparatorios;
    }
}

