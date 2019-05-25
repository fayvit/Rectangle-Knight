using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrifadaFinal : MonoBehaviour
{
    [SerializeField] private GameObject particulaDaBorrifada = default;
    [SerializeField] private GameObject particulaTelegrafista = default;
    [SerializeField] private AudioClip somDasParticulas = default;
    [SerializeField] private Collider2D colisorDeDano = default;
    [SerializeField] private float tempoAteBorrifada=2;
    [SerializeField] private float tempoAteTelegrafia = .5f;
    [SerializeField] private float intervaloDeParticulas = .15f;
    [SerializeField] private float tempoDoBorrifarAoDano = .15f;
    

    private bool iniciarParticulas = false;
    
    // Start is called before the first frame update
    void Start()
    {
        new MyInvokeMethod().InvokeNoTempoDeJogo(()=> { iniciarParticulas = true; Particularizar(); }, tempoAteTelegrafia);
        new MyInvokeMethod().InvokeNoTempoDeJogo(Borrifada,tempoAteBorrifada);
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTrigerInfo);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTrigerInfo);
    }

    void OnReceivedTrigerInfo(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        Collider2D collision = (Collider2D)ssge.MyObject[0];

        if (ssge.Sender.transform.IsChildOf(transform) && collision.tag == "Player" && colisorDeDano.enabled)
        {
            if (collision.tag == "Player")
            {
                if (UnicidadeDoPlayer.Verifique(collision))
                {
                    bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                    EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, 25));
                }
            }
        }

    }

    void Borrifada()
    {
        iniciarParticulas = false;
        InstanciaLigando.Instantiate(particulaDaBorrifada, transform.position, 5);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Darkness4));

        new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
        {
            GetComponent<SpriteRenderer>().color = new Color(0.15f, 0.15f, 0.15f);
            colisorDeDano.enabled = true;
            new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
            {
                if (gameObject != null)
                {
                    colisorDeDano.enabled = false;
                }
            }, .5f);
        }, tempoDoBorrifarAoDano);
    }

    void Particularizar()
    {
        if (gameObject != null)
            if (iniciarParticulas)
            {
                EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, somDasParticulas));
                InstanciaLigando.Instantiate(particulaTelegrafista, transform.position, 2);
                new MyInvokeMethod().InvokeNoTempoDeJogo(Particularizar, intervaloDeParticulas);
            }
    }

}
