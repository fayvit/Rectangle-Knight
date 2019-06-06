using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirculoImperfeitoTrigger : MonoBehaviour
{
    [SerializeField] private BossCirculoImperfeito boss = default;
    [SerializeField] private Transform posicionadorDoHeroi = default;
    [SerializeField] private Transform posDeDeslinicialDoBoss = default;
    [SerializeField] private DadosDeCena.LimitantesDaCena limitantes = default;
    [SerializeField] private GameObject[] barreiras = default;
    [SerializeField] private GameObject particulaDabarreira = default;
    [SerializeField] private GameObject particulaEnfaseDoBoss = default;
    [SerializeField] private AudioClip somEnfaseDoBoss = default;
    [SerializeField] private float changeCamLimitsTime = 0.5f;
    [SerializeField] private float TEMPO_QUEDA_BOSS = 1;
    [SerializeField] private float TEMPO_ATE_INTIMIDACAO = 1;
    [SerializeField] private float TEMPO_GRITANDO = 2F;

    private EstadoDaqui estado = EstadoDaqui.emEspera;
    private Vector3 posInicial;
    private bool limitanteOk;
    private bool heroPositionOk;
    private bool iniciado;
    private float TempoDecorrido = 0;

    private enum EstadoDaqui
    {
        emEspera,
        posicionandoHeroi_limitsCam,
        caindoBoss,
        animaIntimidacao,
        iniciaLuta,
        gritando,
        particulaDaIntimidacao

    }
    // Start is called before the first frame update
    void Start()
    {
        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            posInicial = boss.transform.position;
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyFixedShiftCheck, KeyShift.venceuCirculoImperfeito, gameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.caindoBoss:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido < TEMPO_QUEDA_BOSS)
                {
                    boss.transform.position = Vector3.Lerp(
                        posInicial,
                        posDeDeslinicialDoBoss.position,
                        ZeroOneInterpolation.PolinomialInterpolation(TempoDecorrido / TEMPO_QUEDA_BOSS, 8));
                    
                }
                else
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 10, 2f));
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom,SoundEffectID.pedrasQuebrando));
                    boss.transform.position = posDeDeslinicialDoBoss.position;
                    boss._Animator.SetTrigger("tocouChao");
                    estado = EstadoDaqui.animaIntimidacao;
                    TempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.particulaDaIntimidacao:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > 0.25f)
                {
                    InstanciaLigando.Instantiate(particulaEnfaseDoBoss, boss.transform.position, 5);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somEnfaseDoBoss));
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestLocalnameExibition,"O Grande Círculo Imperfeito",false,2f));

                    new MyInvokeMethod().InvokeNoTempoDeJogo(
                        () => {
                            EventAgregator.Publish(new StandardSendGameEvent(EventKey.startMusic, new NameMusicaComVolumeConfig()
                            {
                                Musica = NameMusic.XPboss3,
                                Volume = 1
                            }
                                ));
                        },1
                        );
                    estado = EstadoDaqui.gritando;
                    TempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.animaIntimidacao:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_ATE_INTIMIDACAO)
                {
                    TempoDecorrido = 0;
                    boss._Animator.SetTrigger("preparaPulo");
                    
                    estado = EstadoDaqui.particulaDaIntimidacao;
                }
            break;
            case EstadoDaqui.gritando:
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > TEMPO_GRITANDO)
                {
                    boss._Animator.SetTrigger("retornaAoPadrao");
                    boss.IniciarBoss();
                    EventAgregator.Publish(EventKey.finalizaDisparaTexto, null);
                    estado = EstadoDaqui.iniciaLuta;
                }
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !iniciado)
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic));
                EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.requestHeroPosition, posicionadorDoHeroi.position));
                EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.requestChangeCamLimits, limitantes,changeCamLimitsTime));

                EventAgregator.AddListener(EventKey.positionRequeredOk, OnHeroPositionOk);
                EventAgregator.AddListener(EventKey.limitCamOk, OnLimitCamOk);

                estado = EstadoDaqui.posicionandoHeroi_limitsCam;
                iniciado = true;

                for (int i = 0; i < barreiras.Length; i++)
                { 
                    barreiras[i].SetActive(true);
                    InstanciaLigando.Instantiate(particulaDabarreira, barreiras[i].transform.position,5);
                }
            }
        }
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.positionRequeredOk, OnHeroPositionOk);
        EventAgregator.RemoveListener(EventKey.limitCamOk, OnLimitCamOk);
    }
    private void VerificaIniciaQuedaDoBoss()
    {

        if (limitanteOk && heroPositionOk)
        {
            new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
            {
                OnDestroy();
            });
            boss._Animator.SetTrigger("queda");
            estado = EstadoDaqui.caindoBoss;
        }
    }

    private void OnLimitCamOk(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        DadosDeCena.LimitantesDaCena l = (DadosDeCena.LimitantesDaCena)ssge.MyObject[0];

        if (limitantes.CompareTo(l)<=Screen.width+Screen.height)
        {
            limitanteOk = true;
            VerificaIniciaQuedaDoBoss();
        }
    }

    private void OnHeroPositionOk(IGameEvent obj)
    {
        if (obj.Sender == gameObject)
        {
            heroPositionOk = true;
            VerificaIniciaQuedaDoBoss();
        }
    }
}