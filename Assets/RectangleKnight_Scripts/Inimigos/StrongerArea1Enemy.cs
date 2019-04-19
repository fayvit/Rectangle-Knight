using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongerArea1Enemy : NonRespawnOnLoadEnemy
{
    [SerializeField] private float coolDown = 1;
    [SerializeField] private float distanciaDeEspadada = 2;
    [SerializeField] private float distanciaDeDesligar = 10;
    [SerializeField] private float tempoDoAtaqueEspada = 1;
    [SerializeField] private float tempoDePorradaNoChao = 1;
    [SerializeField] private float velocidadeDoMovimento = 1.5f;

    #region inspector
    [SerializeField] private Transform limitador_xMin = default;
    [SerializeField] private Transform limitador_xMax = default;
    [SerializeField] private Transform projetilOrigim = default;
    [SerializeField] private Rigidbody2D r2 = default;
    [SerializeField] private GameObject projetil = default;
    [SerializeField] private GameObject particulaTelegrafista = default;
    #endregion

    private float tempoDecorrido = 0;
    private Animator animador;
    private Transform doHeroi;
    private Vector3 posOriginal;
    
    [SerializeField]private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        buscadorDeAcao,
        porradaNoChao,
        ataqueComEspada,
        movimente,
        retorneAoInicio
    }

    protected override void Start()
    {
        animador = GetComponent<Animator>();
        
        posOriginal = transform.position;
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceiveTriggerInfo);
        EventAgregator.AddListener(EventKey.animationPointCheck, OnAnimationPointReceive);

        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceiveTriggerInfo);
        EventAgregator.RemoveListener(EventKey.animationPointCheck, OnAnimationPointReceive);

        base.OnDestroy();
    }

    private void OnAnimationPointReceive(IGameEvent obj)
    {
        Debug.Log(obj.Sender);
        if (obj.Sender == gameObject)
        {
            
            StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
            string animationName = (string)ssge.MyObject[0];

            Debug.Log("animation POint: "+(string)ssge.MyObject[0]);

            if (animationName == "strongerEnemyMagicAttack")
            {
                string receivedInfo = (string)ssge.MyObject[1];


                GameObject G = InstanciaLigando.Instantiate(projetil, projetilOrigim.position, 5,
            Quaternion.LookRotation(-Vector3.forward)
            );
                G.AddComponent<ProjetilInimigo>().IniciarProjetilInimigo(Vector3.right, particulaTelegrafista, 15, SoundEffectID.lancaProjetilInimigo);

                G = InstanciaLigando.Instantiate(projetil, projetilOrigim.position, 5);

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam, ShakeAxis.z, 3, 2f));

                G.AddComponent<ProjetilInimigo>().IniciarProjetilInimigo(-Vector3.right, particulaTelegrafista, 15, SoundEffectID.lancaProjetilInimigo);




            }
            else if (animationName == "ataqueBasicoStrongerArea1_Enemy" || animationName== "padraoStrongerArea1_Enemy")
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.EnemySlash));
            }
        }
    }

    private void OnReceiveTriggerInfo(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if (ssge.Sender.transform.IsChildOf(transform)&& estado==EstadoDaqui.emEspera)
        {
            if (((Collider2D)ssge.MyObject[0]).tag == "Player")
            {
                doHeroi = GameController.g.Manager.transform;
                estado = EstadoDaqui.buscadorDeAcao;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMusicWithRecovery, new NameMusicaComVolumeConfig()
                {
                    Musica = NameMusic.miniBoss,
                    Volume = 1
                }));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (estado)
        {
            case EstadoDaqui.buscadorDeAcao:
                tempoDecorrido += Time.deltaTime;
                animador.SetBool("ataqueMagico", false);
                animador.SetBool("ataqueBasico", false);
                if (tempoDecorrido > coolDown)
                {
                    if (Vector3.Distance(doHeroi.position, transform.position) < distanciaDeDesligar)
                    {
                        if (Mathf.Abs(doHeroi.position.x - transform.position.x) < distanciaDeEspadada)
                        {
                            int sorteio = Random.Range(0, 4);

                            if (sorteio == 0)
                            {
                                DispareMagia();
                            }
                            else
                            {
                                tempoDecorrido = 0;
                                animador.SetBool("ataqueBasico", true);
                                estado = EstadoDaqui.ataqueComEspada;
                            }
                            
                        }
                        else
                        {
                            int sorteioB = Random.Range(0, 4);

                            if (sorteioB == 0)
                            {
                                DispareMagia();
                            }
                            else
                            {
                                estado = EstadoDaqui.movimente;   
                            }
                            
                        }
                    }
                    else
                    {
                        EventAgregator.Publish(EventKey.returnRememberedMusic, null);
                        estado = EstadoDaqui.emEspera;
                    }
                }
            break;
            case EstadoDaqui.movimente:
                float posX = transform.position.x;
                
                if (posX > limitador_xMin.position.x && posX < limitador_xMax.position.x)
                {
                    if (Vector3.Distance(doHeroi.position, transform.position) < distanciaDeEspadada)
                    {
                        tempoDecorrido = coolDown + 1;
                        estado = EstadoDaqui.buscadorDeAcao;
                    }
                    else
                    {
                        r2.velocity = velocidadeDoMovimento* DirecaoNoPlano.NoUpNormalizado(transform.position, doHeroi.position);
                    }
                }
                else
                {
                    if (Vector3.Distance(doHeroi.position, transform.position) > distanciaDeDesligar)
                        estado = EstadoDaqui.retorneAoInicio;
                    else
                    {
                        DispareMagia();
                    }
                }
            break;
            case EstadoDaqui.retorneAoInicio:
                if (Vector3.Distance(doHeroi.position, transform.position) > distanciaDeDesligar)
                {
                    if (Vector3.Distance(transform.position, posOriginal) < 0.5f)
                    {
                        EventAgregator.Publish(EventKey.returnRememberedMusic, null);
                        estado = EstadoDaqui.emEspera;
                    }
                    else
                        r2.velocity = velocidadeDoMovimento * DirecaoNoPlano.NoUpNormalizado(transform.position, posOriginal);
                }
                else
                {
                    tempoDecorrido = coolDown = 1;
                    estado = EstadoDaqui.buscadorDeAcao;
                }
            break;
            case EstadoDaqui.ataqueComEspada:
                
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > tempoDoAtaqueEspada)
                {
                    
                    estado = EstadoDaqui.buscadorDeAcao;
                    animador.SetBool("ataqueBasico", false);
                    tempoDecorrido = 0;
                }

            break;
            case EstadoDaqui.porradaNoChao:

                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > tempoDePorradaNoChao)
                {
                    
                    estado = EstadoDaqui.buscadorDeAcao;
                    animador.SetBool("ataqueMagico", false);
                    tempoDecorrido = 0;
                }

            break;
        }

        transform.position = r2.transform.position;

        if(doHeroi)
            FlipDirection.Flip(transform, -transform.position.x+doHeroi.position.x);
    }

    void DispareMagia()
    {
        tempoDecorrido = 0;
        animador.SetBool("ataqueMagico", true);
        estado = EstadoDaqui.porradaNoChao;
        
    }

    protected override void OnDefeated()
    {
        EventAgregator.Publish(EventKey.returnRememberedMusic, null);
        base.OnDefeated();
    }
}
