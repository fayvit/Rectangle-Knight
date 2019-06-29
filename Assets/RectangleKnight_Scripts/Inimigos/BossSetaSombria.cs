using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetaSombria : EnemyBase
{
    [SerializeField] private GameObject setaSombria = default;
    [SerializeField] private GameObject particulaTelegrafistaDoProjetil = default;
    [SerializeField] private GameObject particulaDoDash = default;
    [SerializeField] private GameObject particulaDoTeleport = default;
    [SerializeField] private GameObject downArrow = default;
    [SerializeField] private GameObject downArrowGroundParticles = default;
    [SerializeField] private GameObject diagonalDownArrow = default;
    [SerializeField] private GameObject[] especiais = default;
    [SerializeField] private GameObject enfaseDoBoss = default;
    [SerializeField] private Transform[] posAereas = default;
    [SerializeField] private Transform[] posNoChao = default;
    [SerializeField] private Transform[] posDeSalto = default;
    [SerializeField] private AudioClip uaDoMago = default;
    [SerializeField] private AudioClip oioaioio = default;
    [SerializeField] private AudioClip risada = default;
    [SerializeField] private Collider2D colliderNormal = default;
    [SerializeField] private Collider2D colliderDash = default;


    [SerializeField] private float coolDownInit = 1;
    [SerializeField] private float coolDownFinish = 2;
    [SerializeField] private float intervaloDeTeleport = 0.75f;
    [SerializeField] private float velDescida = 5;
    [SerializeField] private float velDash = 10;
    [SerializeField] private float tempoAposTocarChao = 0.75f;
    [SerializeField] private float tempoPosMagiaSimples = 2;
    [SerializeField] private float velDaMagia = 15;
    [SerializeField] private float tempoMinimoEntreEspeciais = 120;
    [SerializeField] private float tempoAposDash = .65f;
    [SerializeField] private float tempoDoGritoAoEspecial = 1.5f;
    [SerializeField] private float tempoTelegrafandoEspecial=.75f;
    [SerializeField] private int totalDeAcionamentosEspeciais = 4;
    [SerializeField] private int totalDeEspeciaisPorAcionamento = 4;



    private EstadoDaqui estado = EstadoDaqui.emEspera;
    private Vector3 posInicial;
    private Vector3 posFinal;
    private GameObject meuObjeto;
    private float distanciaEntrePontos = 0;
    private float tempoDecorrido = 0;
    private float tempoDoUltimoEspecial = 0;
    private int cont = 0;
    private int contadorDeAcionamentos = 0;


    private enum EstadoDaqui
    {
        emEspera,
        emCoolDown,
        atacando,
        emTeleport,
        emDescidaReta,
        diagonal,
        preparandoMagiaSimples,
        preparandoMagiaMultipla,
        executandoMagiaMultipla,
        preparandoAtkEspecial,
        executandoAtkEspecial,
        dash,
        derrotado
    }

    private enum BossAttack
    {
        descidaReta,
        diagonal,
        magiaSimples,
        magiaMultipla,
        dash,
        atkEspecial
    }

    private enum AnimKey
    {
        magia,
        sairDaMagia,
        diagonal,
        tocouChao,
        tocouChaoParaPadrao,
        descidaReta,
        dash,
        saiuDoDash
    }

    public Vector3 HeroPosition { get => GameController.g.Manager.transform.position; }


    protected override void Start()
    {
       // new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciaBoss, 0.3f);
        tempoDoUltimoEspecial = tempoMinimoEntreEspeciais;
        EventAgregator.AddListener(EventKey.animationPointCheck, OnReceivedAnimationPoint);
        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.animationPointCheck, OnReceivedAnimationPoint);
        base.OnDestroy();
    }

    protected override void OnDefeated()
    {
        estado = EstadoDaqui.derrotado;
        FindObjectOfType<FinalDoSetaSombria>().IniciarFinalDoSetaSombria(this);
    }

    void OnReceivedAnimationPoint(IGameEvent e)
    {
        Debug.Log("animation point received: " + e.Sender);
        if (e.Sender == gameObject)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;

            string s = (string)ssge.MyObject[1];
            Debug.Log("animation point info: " + s);

            switch (s)
            {
                case "reta":
                    estado = EstadoDaqui.emDescidaReta;
                    posInicial = transform.position;
                    posFinal = new Vector3(transform.position.x, posNoChao[0].position.y-1, transform.position.z);
                    distanciaEntrePontos = Vector2.Distance(posInicial, posFinal);
                    downArrow.SetActive(true);
                    tempoDecorrido = 0;
                    break;
                case "diagonal":
                    colliderNormal.enabled = false;
                    colliderDash.enabled = true;
                    estado = EstadoDaqui.diagonal;
                    distanciaEntrePontos = Vector2.Distance(posInicial, posFinal);
                    diagonalDownArrow.SetActive(true);
                    tempoDecorrido = 0;
                    break;
                case "magia":
                    switch (estado)
                    {
                        case EstadoDaqui.preparandoMagiaSimples:
                            SpawnarMagia(transform.position - 1.5f * Vector3.up);
                            estado = EstadoDaqui.emEspera;
                            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciaBoss, tempoPosMagiaSimples);
                        break;
                        case EstadoDaqui.preparandoMagiaMultipla:
                            estado = EstadoDaqui.executandoMagiaMultipla;
                            posInicial = transform.position;
                            VerificaDisparo();
                        break;
                    }

                    _Animator.SetTrigger(AnimKey.sairDaMagia.ToString());

                    break;
                case "dash":
                    colliderDash.enabled = true;
                    colliderNormal.enabled = false;

                    estado = EstadoDaqui.dash;
                    distanciaEntrePontos = Vector2.Distance(posInicial, posFinal);
                    tempoDecorrido = 0;
                    particulaDoDash.SetActive(true);
                break;
            }


        }
    }

    void VerificaDisparo()
    {
        switch (cont)
        {
            case 0:
                if (Vector3.Distance(posInicial, posNoChao[0].position) < Vector3.Distance(posInicial, posNoChao[1].position))
                    posFinal = posDeSalto[0].position;
                else
                    posFinal = posDeSalto[1].position;


                SpawnarMagia(transform.position - 1.5f * Vector3.up);
            break;
            case 1:
                if (Vector3.Distance(posInicial, posDeSalto[0].position) < Vector3.Distance(posInicial, posDeSalto[1].position))
                    posFinal = posAereas[0].position;
                else
                    posFinal = posAereas[1].position;


                SpawnarMagia(transform.position - 1.5f * Vector3.up);
            break;
            case 2:
                if (Vector3.Distance(posInicial, posAereas[0].position) < Vector3.Distance(posInicial, posAereas[1].position))
                    posFinal = posNoChao[0].position;
                else
                    posFinal = posNoChao[1].position;

                SpawnarMagia(transform.position - 1.5f * Vector3.up, HeroPosition - transform.position);
            break;
            case 3:
                SpawnarMagia(transform.position - 1.5f * Vector3.up);

                new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,
                    () =>
                    {
                        Debug.Log(estado);
                        if (estado == EstadoDaqui.executandoMagiaMultipla || estado == EstadoDaqui.preparandoMagiaMultipla)
                        {
                            estado = EstadoDaqui.emEspera;
                            IniciaBoss();
                        }
                    }, tempoPosMagiaSimples);
            break;

        }

        cont++;
        tempoDecorrido = 0;
        distanciaEntrePontos = Vector3.Distance(posInicial, posFinal);

    }

    void SpawnarMagia(Vector3 pos, Vector3 dir = default)
    {
        if (dir == default)
            dir = DirecaoNoPlano.NoUpNormalizado(transform.position, HeroPosition);

        GameObject G = InstanciaLigando.Instantiate(setaSombria, pos, 10);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));

        G.AddComponent<ProjetilInimigo>().IniciarProjetilInimigo(dir,
            particulaDoTeleport, velDaMagia, SoundEffectID.lancaProjetilInimigo);


        G.transform.rotation = Rotation2D.GetRotation(dir);


    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.emDescidaReta:
                tempoDecorrido += Time.deltaTime;
                transform.position = Vector3.Lerp(posInicial, posFinal,
                    ZeroOneInterpolation.PolinomialInterpolation(tempoDecorrido / distanciaEntrePontos * velDescida, 2)
                    );

                if (tempoDecorrido >= distanciaEntrePontos / velDescida)
                {
                    ComunsDeImpactoChao();
                    downArrow.SetActive(false);
                }
                break;
            case EstadoDaqui.diagonal:
                tempoDecorrido += Time.deltaTime;
                transform.position = Vector3.Lerp(posInicial, posFinal,
                    ZeroOneInterpolation.PolinomialInterpolation(tempoDecorrido / distanciaEntrePontos * velDescida, 2)
                    );
                if (tempoDecorrido >= distanciaEntrePontos / velDescida)
                {
                    colliderNormal.enabled = true;
                    colliderDash.enabled = false;

                    ComunsDeImpactoChao();
                    diagonalDownArrow.SetActive(false);

                }
                break;
            case EstadoDaqui.executandoMagiaMultipla:

                tempoDecorrido += Time.deltaTime;

                transform.position = Vector3.Lerp(posInicial, posFinal,
                    ZeroOneInterpolation.PolinomialInterpolation(tempoDecorrido / distanciaEntrePontos * velDescida, 2)
                    );

                if (tempoDecorrido >= distanciaEntrePontos / velDescida)
                {
                    _Animator.SetTrigger(AnimKey.magia.ToString());
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.preparandoMagiaMultipla;
                }
                break;
            case EstadoDaqui.dash:
                tempoDecorrido += Time.deltaTime;

                transform.position = Vector3.Lerp(posInicial, posFinal,
                    ZeroOneInterpolation.PolinomialInterpolation(tempoDecorrido / distanciaEntrePontos * velDash, 2)
                    );

                if (tempoDecorrido >= distanciaEntrePontos / velDash)
                {
                    colliderDash.enabled = false;
                    colliderNormal.enabled = true;

                    estado = EstadoDaqui.emEspera;
                    _Animator.SetTrigger(AnimKey.saiuDoDash.ToString());
                    particulaDoDash.SetActive(false);
                    new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciaBoss, tempoAposDash);
                }
                break;
        }
    }

    void ComunsDeImpactoChao()
    {
        _Animator.SetTrigger(AnimKey.tocouChao.ToString());
        estado = EstadoDaqui.emEspera;

        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,

            IniciaBoss
        , tempoAposTocarChao);

        _Animator.SetTrigger(AnimKey.tocouChaoParaPadrao.ToString());

        InstanciaLigando.Instantiate(downArrowGroundParticles, transform.position + 2 * Vector3.down, 5);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.pedrasQuebrando));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.x, 5, 2f));
    }

    void PosTeleportParaAlto()
    {
        if (estado == EstadoDaqui.emTeleport)
        {
            EscolhaPosAereaParaChefeChefe();

            InvocaTeleportProps(true);
            estado = EstadoDaqui.emCoolDown;

            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PosCoolDown, Random.Range(coolDownInit, coolDownFinish));
        }
    }

    void EscolhaPosAereaParaChefeChefe()
    {
        Vector3 posAlvo = posAereas[0].position;

        for (int i = 1; i < posAereas.Length; i++)
            if (Vector3.Distance(posAereas[i].position, HeroPosition) < Vector3.Distance(posAlvo, HeroPosition))
                posAlvo = posAereas[i].position;

        transform.position = posAlvo;
    }

    bool VerifiqueAtkEspecial()
    {/*
        Debug.Log("PONTOS DE VIDA: "+
            Dados.MaxVida * (1 - ((float)contadorDeAcionamentos + 1) / (totalDeAcionamentosEspeciais + 1))+
            " : "+ (1 - ((float)contadorDeAcionamentos + 1) / (totalDeAcionamentosEspeciais + 1))+" : "+contadorDeAcionamentos+" : "+totalDeAcionamentosEspeciais);
            */
        if (Dados.PontosDeVida < Dados.MaxVida * (1 - ((float)contadorDeAcionamentos + 1) / (totalDeAcionamentosEspeciais + 1))
            &&
            Dados.PontosDeVida > 0
            &&
            Time.time - tempoDoUltimoEspecial > tempoMinimoEntreEspeciais
            )
        {
            tempoDoUltimoEspecial = Time.time;
            contadorDeAcionamentos++;
            return true;
        }
        return false;
    }

    void PosCoolDown()
    {
        if (estado == EstadoDaqui.emCoolDown)
        {

            if (!VerifiqueAtkEspecial())
            {
                int sorteio = Random.Range(0, 5);
                BossAttack b = (BossAttack)sorteio;

                //Debug.Log("numero de sorteio: "+sorteio+" enum bossAttack: "+b+" : "+estado);
                switch (b)
                {
                    case BossAttack.descidaReta:
                        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciarDescidaReta, intervaloDeTeleport);
                    break;
                    case BossAttack.diagonal:
                        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciarDescidaDiagonal, intervaloDeTeleport);
                    break;
                    case BossAttack.magiaSimples:
                        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PreparMagiaSimples, intervaloDeTeleport);
                    break;
                    case BossAttack.magiaMultipla:
                        cont = 0;
                        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PreparaMagiaMultipla, intervaloDeTeleport);
                    break;
                    case BossAttack.dash:
                        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciaDash, intervaloDeTeleport);
                    break;
                }

                InvocaTeleportProps(false);
                estado = EstadoDaqui.atacando;
            }
            else
            {
                IniciarAtkEspecial();
            }
            //Debug.Log("estado: " + estado);
        }
    }

    void IniciarAtkEspecial()
    {
        InvocaTeleportProps(false);
        estado = EstadoDaqui.preparandoAtkEspecial;
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, RisadaPreEspecial, intervaloDeTeleport);
    }

    void RisadaPreEspecial()
    {
        if (estado == EstadoDaqui.preparandoAtkEspecial)
        {
            cont = 0;
            EscolhaPosAereaParaChefeChefe();
            InvocaTeleportProps(true);
            InstanciaLigando.Instantiate(enfaseDoBoss, transform.position, 10);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 10, 2f));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, risada));
            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PrepararInstanciaDeEspecial, tempoDoGritoAoEspecial);
        }
    }

    void PrepararInstanciaDeEspecial()
    {
        if(cont >0)
            _Animator.SetTrigger(AnimKey.sairDaMagia.ToString());

        if (estado == EstadoDaqui.preparandoAtkEspecial && cont < totalDeEspeciaisPorAcionamento)
        {
            cont++;
            SelecioneUmEspecial();
            for (int i = 0; i < meuObjeto.transform.childCount; i++)
            {
                Vector3 V = meuObjeto.transform.GetChild(i).position;
                InstanciaLigando.Instantiate(particulaTelegrafistaDoProjetil, V, 5);
            }

            _Animator.SetTrigger(AnimKey.magia.ToString());
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 5, 1f));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom,SorteieSom(true)));
            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, InstanciarSeta, tempoTelegrafandoEspecial);
        }
        else if (cont >= totalDeEspeciaisPorAcionamento)
        {
            estado = EstadoDaqui.emEspera;
            IniciaBoss();
        }
    }

    void InstanciarSeta()
    {
        
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));
        InstanciaLigando.Instantiate(meuObjeto, meuObjeto.transform.position, 10);
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PrepararInstanciaDeEspecial, tempoDoGritoAoEspecial);
    }

    void SelecioneUmEspecial()
    {
        int qual =  Random.Range(0, especiais.Length);
        meuObjeto = especiais[qual];
    }

    void IniciaDash()
    {
        if (estado == EstadoDaqui.atacando)
        {

            posInicial = posNoChao[0].position;
            posFinal = posNoChao[1].position;

            if (Vector3.Distance(HeroPosition, posNoChao[0].position) < Vector3.Distance(HeroPosition, posNoChao[1].position))
            {
                posInicial = posNoChao[1].position;
                posFinal = posNoChao[0].position;
            }

            transform.position = posInicial;
            
            //transform.position +=;
            PreparaAttack(false, AnimKey.dash);
        }
    }

    void PreparaMagiaMultipla()
    {
        if (estado == EstadoDaqui.atacando)
        {
            ComunsNaPreparacaoDeMagia();
            estado = EstadoDaqui.preparandoMagiaMultipla;
        }
    }

    void PreparMagiaSimples()
    {
        if (estado == EstadoDaqui.atacando)
        {
            ComunsNaPreparacaoDeMagia();
            estado = EstadoDaqui.preparandoMagiaSimples;
        }
    }

    void ComunsNaPreparacaoDeMagia()
    {
        posInicial = posNoChao[0].position;

        if (Vector3.Distance(posInicial, HeroPosition) < Vector3.Distance(posNoChao[1].position, HeroPosition))
            posInicial = posNoChao[1].position;

        transform.position = posInicial;

        PreparaAttack(true, AnimKey.magia);
        
    }

    void PreparaAttack(bool magia,AnimKey a)
    {
        InvocaTeleportProps(true);
        _Animator.SetTrigger(a.ToString());
        FlipDirection.Flip(transform, -(HeroPosition.x - transform.position.x));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SorteieSom(magia)));
    }

    void IniciarDescidaDiagonal()
    {
        if (estado == EstadoDaqui.atacando)
        {
            posInicial = new Vector3(HeroPosition.x+posAereas[0].position.y-posNoChao[0].position.y,posAereas[0].position.y,0);
            posFinal = new Vector3(HeroPosition.x, posAereas[0].position.y, 0);
            RaycastHit2D hit = Physics2D.Linecast(posInicial, posFinal);

            if(hit)
                if(hit.transform.gameObject.layer==8)
                    posInicial = new Vector3(HeroPosition.x - posAereas[0].position.y + posNoChao[0].position.y, posAereas[0].position.y, 0);

            transform.position = posInicial;
            posFinal = new Vector3(HeroPosition.x, posNoChao[0].position.y-2, transform.position.z);//será usado posteriormente

            PreparaAttack(false, AnimKey.diagonal);
        }
    }

    void IniciarDescidaReta()
    {
        if (estado == EstadoDaqui.atacando)
        {
            
            transform.position = HeroPosition + Vector3.up * 10;
            //posInicial = transform.position;
            //posFinal = new Vector3(HeroPosition.x, posNoChao[0].position.y, 0);
            //transform.position +=;
            PreparaAttack(false, AnimKey.descidaReta);
        }
    }

    AudioClip SorteieSom(bool magia)
    {
        float sorteio = Random.value;

        if ((magia && sorteio < .8f) || (!magia && sorteio >= .8f))
        {
            return oioaioio;
        }
        else
            return uaDoMago;
    }

    void IniciaBoss()
    {
        if (estado == EstadoDaqui.emEspera)
        {
            estado = EstadoDaqui.emTeleport;

            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, PosTeleportParaAlto, intervaloDeTeleport);


            InvocaTeleportProps(false);
        }
    }

    public void InvocaTeleportProps(bool ligar)
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.aparicaoSurpresaDeInimigo));
        InstanciaLigando.Instantiate(particulaDoTeleport, transform.position, 5);
        gameObject.SetActive(ligar);
    }

    public void IniciaApresentacaoDoBoss(Vector3 posParaParticulaDeOrigem)
    {
        GameController.g.LocalName.RequestLocalNameExibition("O mago Seta Sombria", false,3);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.aparicaoSurpresaDeInimigo));
        EventAgregator.Publish(EventKey.abriuPainelSuspenso);
        InstanciaLigando.Instantiate(particulaDoTeleport, posParaParticulaDeOrigem, 5);
        new MyInvokeMethod().InvokeNoTempoDeJogo(AparicaoPreRisada, .5f);
    }

    void AparicaoPreRisada()
    {
        gameObject.SetActive(true);
        InstanciaLigando.Instantiate(particulaDoTeleport, transform.position, 5);
        new MyInvokeMethod().InvokeNoTempoDeJogo(RisadaDeApresentacao, .5f);
    }

    void RisadaDeApresentacao()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, risada));
        InstanciaLigando.Instantiate(enfaseDoBoss, transform.position, 10);
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, IniciarLuta, tempoDoGritoAoEspecial);
    }

    void IniciarLuta()
    {
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMusicWithRecovery,new NameMusicaComVolumeConfig() { Musica = NameMusic.XPBoss4, Volume = 1 }));
        IniciaBoss();
    }
}
