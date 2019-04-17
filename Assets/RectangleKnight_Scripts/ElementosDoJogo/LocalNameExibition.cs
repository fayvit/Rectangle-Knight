using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalNameExibition : MonoBehaviour
{
    [SerializeField] private GameObject grandeDestaque = default;
    [SerializeField] private GameObject nomeDiscreto = default;
    [SerializeField] private string ID;
    [SerializeField] private bool sempreDiscreto = false;

    private Image[] imgs;
    private Text txt;
    private EstadoDaqui estado = EstadoDaqui.emEspera;
    private float tempoDecorrido = 0;

    private const float TEMPO_DE_FADE = 1f;
    private const float TEMPO_MOSTRANDO = 5;

    private enum EstadoDaqui
    {
        emEspera,
        fadeIn,
        mostrando,
        fadeOut,
        concluido
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.fadeIn:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido <= TEMPO_DE_FADE)
                {
                    foreach (Image img in imgs)
                    {
                        Color C = img.color;
                        C.a = Mathf.Lerp(0, 1, tempoDecorrido / TEMPO_DE_FADE);
                        img.color = C;
                    }

                    Color Cc = txt.color;
                    Cc.a = Mathf.Lerp(0, 1, tempoDecorrido / TEMPO_DE_FADE);
                    txt.color = Cc;
                }
                else
                {
                    foreach (Image img in imgs)
                    {
                        Color C = img.color;
                        C.a = 1;
                        img.color = C;
                    }

                    Color Cc = txt.color;
                    Cc.a = 1;
                    txt.color = Cc;

                    estado = EstadoDaqui.mostrando;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.mostrando:

                tempoDecorrido += Time.deltaTime;

                if (tempoDecorrido > TEMPO_MOSTRANDO)
                {
                    estado = EstadoDaqui.fadeOut;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.fadeOut:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido <= TEMPO_DE_FADE)
                {
                    foreach (Image img in imgs)
                    {
                        Color C = img.color;
                        C.a = Mathf.Lerp(1, 0, tempoDecorrido / TEMPO_DE_FADE);
                        img.color = C;
                    }

                    Color Cc = txt.color;
                    Cc.a = Mathf.Lerp(1, 0, tempoDecorrido / TEMPO_DE_FADE);
                    txt.color = Cc;
                }
                else
                {
                    foreach (Image img in imgs)
                    {
                        Color C = img.color;
                        C.a = 0;
                        img.color = C;
                    }

                    Color Cc = txt.color;
                    Cc.a = 0;
                    txt.color = Cc;

                    estado = EstadoDaqui.concluido;
                    tempoDecorrido = 0;
                }
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && estado==EstadoDaqui.emEspera)
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                if (sempreDiscreto || GameController.g.MyKeys.VerificaAutoShift(ID))
                {
                    imgs = nomeDiscreto.GetComponentsInChildren<Image>();
                    txt = nomeDiscreto.GetComponentInChildren<Text>();
                }
                else
                {
                    imgs = grandeDestaque.GetComponentsInChildren<Image>();
                    txt = grandeDestaque.GetComponentInChildren<Text>();
                }

                foreach (Image img in imgs)
                {
                    Color C = img.color;
                    C.a = 0;
                    img.color = C;
                }

                Color Cc = txt.color;
                Cc.a = 0;
                txt.color = Cc;

                estado = EstadoDaqui.fadeIn;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
            }
        }
    }
}
