using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyFadeView : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Image escurecedorUpper;
    [SerializeField] private Image escurecedorLower;
#pragma warning restore 0649
    private Color corDoFade = Color.black;
    private FaseDaqui fase = FaseDaqui.emEspera;
    private float tempoDeEscurecimento = 1;
    private float tempoDecorrido = 0;
    private System.Action acaoDoFade;


    public bool escurecer = false;
    public bool clarear = false;

    private enum FaseDaqui
    {
        emEspera,
        escurecendo,
        clareando
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (escurecer)
        {
            IniciarFadeOut();
            escurecer = false;
        }

        if (clarear)
        {
            IniciarFadeIn();
            clarear = false;
        }
        switch (fase)
        {
            case FaseDaqui.escurecendo:
                tempoDecorrido += Time.fixedDeltaTime;
                corDoFade.a = tempoDecorrido / tempoDeEscurecimento;
                escurecedorUpper.color = corDoFade;
                escurecedorLower.color = corDoFade;

                if (tempoDecorrido > tempoDeEscurecimento)
                {
                    fase = FaseDaqui.emEspera;
                    ChamarAcao();
                    EventAgregator.Publish(EventKey.fadeOutComplete, null);
                }
            break;
            case FaseDaqui.clareando:
                tempoDecorrido += Time.fixedDeltaTime;
                corDoFade.a = (tempoDeEscurecimento - tempoDecorrido) / tempoDeEscurecimento;
                escurecedorUpper.color = corDoFade;
                escurecedorLower.color = escurecedorUpper.color;
                if (tempoDecorrido > tempoDeEscurecimento)
                {
                    fase = FaseDaqui.emEspera;
                    EventAgregator.Publish(EventKey.fadeInComplete, null);

                    ChamarAcao();

                    escurecedorLower.gameObject.SetActive(false);
                    escurecedorUpper.gameObject.SetActive(false);
                }
            break;
        }
    }

    public void LimparFade()
    {
        escurecedorUpper.color = new Color(0, 0, 0, 0);
        escurecedorLower.color = new Color(0, 0, 0, 0);
    }

    public void IniciarFadeOut(Color corDoFade = default(Color))
    {
        ComunsDeFadeOut(corDoFade);
        EventAgregator.Publish(EventKey.fadeOutStart, null);
    }

    void ComunsDeFadeOut(Color corDoFade)
    {
        escurecedorLower.gameObject.SetActive(true);
        escurecedorUpper.gameObject.SetActive(true);
        this.corDoFade = corDoFade;
        this.corDoFade.a = 0;
        fase = FaseDaqui.escurecendo;
        tempoDecorrido = 0;
    }

    public void IniciarFadeOutComAction(System.Action acao, Color corDoFade = default)
    {
        IniciarFadeOutComAction(acao,1,corDoFade);
    }

    public void IniciarFadeOutComAction(System.Action acao, float tempoDeEscurecimento,Color corDoFade = default)
    {
        this.tempoDeEscurecimento = tempoDeEscurecimento;
        ComunsDeFadeOut(corDoFade);
        StartCoroutine(AcaoDequadro(acao));
    }

    IEnumerator AcaoDequadro(System.Action acao)
    {
        yield return new WaitForEndOfFrame();
        acaoDoFade += acao;
    }

    void ChamarAcao()
    {
        if (acaoDoFade != null)
        {
            acaoDoFade();
            acaoDoFade = null;
        }
    }

    public void IniciarFadeInComAction(System.Action acao, Color corDoFade = default)
    {
        IniciarFadeInComAction(acao, 1, corDoFade);
    }

    public void IniciarFadeInComAction(System.Action acao, float tempoDeEscurecimento, Color corDoFade = default)
    {
        this.tempoDeEscurecimento = tempoDeEscurecimento;
        ComunsDoFadeIn(corDoFade);
        StartCoroutine(AcaoDequadro(acao));
    }

    void ComunsDoFadeIn(Color corDoFade)
    {
        this.corDoFade = corDoFade;
        this.corDoFade.a = 1;
        fase = FaseDaqui.clareando;
        tempoDecorrido = 0;
    }

    public void IniciarFadeIn(Color corDoFade = default(Color))
    {
        ComunsDoFadeIn(corDoFade);
        EventAgregator.Publish(EventKey.fadeInStart, null);
    }
}
