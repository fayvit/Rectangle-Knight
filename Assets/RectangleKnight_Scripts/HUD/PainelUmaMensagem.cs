using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PainelUmaMensagem : MonoBehaviour
{
    public delegate void RetornarParaAntecessor();
    public event RetornarParaAntecessor retornar;

#pragma warning disable 0649
    [SerializeField] private Text textoDaMensagem;
    [SerializeField] private Text textoDoBotao;
#pragma warning restore 0649
    // Use this for initialization
    public void ConstroiPainelUmaMensagem(RetornarParaAntecessor r, string textoDaMensagem)
    {
        //ActionManager.ModificarAcao(transform, BotaoEntendi);
        gameObject.SetActive(true);
        this.textoDaMensagem.text = textoDaMensagem;
        retornar = r;
    }

    public void ConstroiPainelUmaMensagem(RetornarParaAntecessor r)
    {
        gameObject.SetActive(true);
        retornar = r;
    }

    public void AtualizarTextoDaMensagem(string s)
    {
        textoDaMensagem.text = s;
    }

    public void AtualizaTextoDoBotao(string s)
    {
        textoDoBotao.text = s;
    }

    public void AtualizaTextoDaMensagemEBotao(string textoDoBotao, string textoDaMensagem)
    {
        AtualizarTextoDaMensagem(textoDaMensagem);
        AtualizaTextoDoBotao(textoDoBotao);
    }

    public void BotaoEntendi()
    {
        ActionManager.ModificarAcao(null, null);
        gameObject.SetActive(false);
        if (retornar != null)
        {
            retornar();
            retornar = null;
        }

    }

    void Update()
    {
        if (ActionManager.ButtonUp(1, GameController.g.Manager.Control) || ActionManager.ButtonUp(0, GameController.g.Manager.Control))
        {
            Debug.Log("disparaacao e nao cancel");
            EventAgregator.Publish(EventKey.negativeUiInput, null);
            BotaoEntendi();
        }
    }
}
