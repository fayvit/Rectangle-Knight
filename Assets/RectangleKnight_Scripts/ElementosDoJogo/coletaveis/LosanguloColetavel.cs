using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosanguloColetavel : ActiveFalseForShift
{
    #region inspector
    [SerializeField] private GameObject particulaDaColeta = default;
    [SerializeField] private ParticleSystem particulaGeiser = default;
    #endregion

    private bool iniciado = false;
    private Animator animador;
    // Start is called before the first frame update
    protected override void Start()
    {
        animador = GetComponent<Animator>();
        base.Start();

        EventAgregator.AddListener(EventKey.animationPointCheck, OnReceiveAnimationPoint);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.animationPointCheck, OnReceiveAnimationPoint);
    }

    void OnReceiveAnimationPoint(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;


        if (e.Sender == gameObject && (string)ssge.MyObject[1] != "secondSound")
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.somParaEruptLosangulo));
            particulaGeiser.gameObject.SetActive(true);
            Invoke("FinalizaColeta", 2);
        }
        else if (e.Sender == gameObject && (string)ssge.MyObject[1] == "secondSound")
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, "Wind1"));
        }
    }

    void FinalizaColeta()
    {
        particulaGeiser.Stop();
        InstanciaLigando.Instantiate(particulaDaColeta, transform.position, 5);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.somParaGetLosangulo));
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !iniciado)
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                animador.SetTrigger("coletado");
                InstanciaLigando.Instantiate(particulaDaColeta, transform.position);
                iniciado = true;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.sumContShift, KeyCont.losangulosPegos, 1));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.somParaGetLosangulo));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, "Wind1"));
                // Coletou();
            }
        }
    }
}
