using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcultInvestEnemy : NonRespawnOnLoadEnemy
{
    [System.Serializable]
    private struct Deslocamento
    {
#pragma warning disable 0649
        public Transform partida;
        public Transform chegada;
#pragma warning restore 0649
    }

    [SerializeField] private GameObject particulaDoAparecimento = default;
    [SerializeField] private GameObject particulaDoDeslocamento = default;
    [SerializeField] private SpriteRenderer meuSprite = default;
    [SerializeField] private Collider2D meuCollider = default;
    [SerializeField] private Deslocamento[] trajetorias = default;
    [SerializeField] private float vel = 10;
    [SerializeField] private float intervaloDeInvestimento = 4;
    [SerializeField] private float tempoTelegrafando = .5f;
    [SerializeField] private float disMin = 3;

    private float ultimoInvestimento = 0;
    private float tempoDecorrido = 0;
    private int indiceDoDeslocamento = 0;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        telegrafando,
        investindo
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        ChangeView(false);
        ultimoInvestimento = -intervaloDeInvestimento;
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.Start();
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.OnDestroy();
        ChangeView(false);
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        if (transform.IsChildOf(e.Sender.transform) && estado==EstadoDaqui.emEspera)
        {
            float px = ((Collider2D)((StandardSendGameEvent)e).MyObject[0]).transform.position.x;
            bool foi = false;

            if (Mathf.Abs(ultimoInvestimento - Time.time) > intervaloDeInvestimento)
            {
                for (int i = 0; i < trajetorias.Length; i++)
                {
                    float partida = trajetorias[i].partida.position.x;
                    float chegada = trajetorias[i].chegada.position.x;
                    //Debug.Log(Mathf.Abs(partida - px)+" : "+px +" : "+" : "+partida+" : "+chegada+" : "+(partida < px && px < chegada) +" : "+ (partida > px && chegada < px) +" : "+ (Mathf.Abs(partida - px) > disMin)+" : "+i);
                    if (((partida < px && px < chegada) || (partida > px && chegada < px))&&(Mathf.Abs(partida-px)>disMin))
                    {
                        indiceDoDeslocamento = i;
                        foi = true;
                    }
                }

                if (foi)
                {
                    transform.position = trajetorias[indiceDoDeslocamento].partida.position;
                    InstanciaLigando.Instantiate(particulaDoAparecimento, transform.position, 5);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.aparicaoSurpresaDeInimigo));
                    estado = EstadoDaqui.telegrafando;
                    tempoDecorrido = 0;
                    
                    FlipDirection.Flip(transform, 
                        trajetorias[indiceDoDeslocamento].partida.position.x - trajetorias[indiceDoDeslocamento].chegada.position.x);
                    ChangeView(true);
                }
            }
        }
    }

    void ChangeView(bool b)
    {
        meuSprite.enabled = b;
        meuCollider.enabled = b;
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.investindo:
                tempoDecorrido += Time.deltaTime;
                Vector3 partida = trajetorias[indiceDoDeslocamento].partida.position;
                Vector3 chegada = trajetorias[indiceDoDeslocamento].chegada.position;
                float dist = Vector3.Distance(partida, chegada);
                transform.position = Vector3.Lerp(partida, chegada, tempoDecorrido * vel / dist);

                if (tempoDecorrido * vel / dist > 1)
                {
                    estado = EstadoDaqui.emEspera;
                    ultimoInvestimento = Time.time;
                    InstanciaLigando.Instantiate(particulaDoAparecimento, transform.position, 5);
                    EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, SoundEffectID.aparicaoSurpresaDeInimigo));
                    particulaDoDeslocamento.SetActive(false);
                    ChangeView(false);
                }
            break;
            case EstadoDaqui.telegrafando:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > tempoTelegrafando)
                {
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.investindo;
                    particulaDoDeslocamento.SetActive(true);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.avancoDoInimigo));
                }
            break;
        }
    }
}
