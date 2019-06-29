using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordColorManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private MyButtonEvents[] colorButtons;
    [SerializeField] private Sprite doSelecionado;
    [SerializeField] private Sprite doPadrao;
#pragma warning restore 0649

    private Sprite DoSelecionado { get => doSelecionado; set => doSelecionado = value; }

    // Start is called before the first frame update
    void Start()
    {
        EventAgregator.AddListener(EventKey.colorChanged, OnColorChanged);
        EventAgregator.AddListener(EventKey.getColorSword, OnGetColorSword);
        EventAgregator.AddListener(EventKey.colorSwordShow, OnRequestFillDates);
        EventAgregator.AddListener(EventKey.allAbilityOn, VerifiqueEspadasAtivas);
        EventAgregator.AddListener(EventKey.starterHudForTest, VerifiqueEspadasAtivas);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.colorChanged, OnColorChanged);
        EventAgregator.RemoveListener(EventKey.getColorSword, OnGetColorSword);
        EventAgregator.RemoveListener(EventKey.colorSwordShow, OnRequestFillDates);
        EventAgregator.RemoveListener(EventKey.allAbilityOn, VerifiqueEspadasAtivas);
        EventAgregator.RemoveListener(EventKey.starterHudForTest, VerifiqueEspadasAtivas);
    }

    private void VerifiqueEspadasAtivas(IGameEvent e)
    {
        new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
        {
            OnGetColorSword(null);
        });
    }

    void OnRequestFillDates(IGameEvent e)
    {
        EscolheQualCorMostrar();
    }

    void OnGetColorSword(IGameEvent e)
    {
        GlobalController.g.StartCoroutine(EscolheQualCorMostrarNoProximoQuadro());
    }

    void OnColorChanged(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if ((int)ssge.MyObject[0] == i)
                colorButtons[i].GetComponent<Image>().sprite = DoSelecionado;
            else
                colorButtons[i].GetComponent<Image>().sprite = doPadrao;

        }

    }

    IEnumerator EscolheQualCorMostrarNoProximoQuadro()
    {
        yield return new WaitForEndOfFrame();
        EscolheQualCorMostrar();
    }

    void EscolheQualCorMostrar()
    {

        bool foi = false;
        DadosDoJogador d = GameController.g.Manager.Dados;
        for (int i = 1; i < colorButtons.Length; i++)
        {
            //Debug.Log(GameController.g.Manager.Dados.SwordAvailable((SwordColor)i)+" : "+(SwordColor)i);
            if (d.SwordAvailable((SwordColor)i))
            {
                colorButtons[i].gameObject.SetActive(true);
                foi = true;
            }
            else
            {
                if(colorButtons[i])
                    colorButtons[i].gameObject.SetActive(false);
            }
        }

        if (colorButtons[0])
            colorButtons[0].gameObject.SetActive(foi);

        if (foi)
            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (d.CorDeEspadaSelecionada == (SwordColor)i)
                    colorButtons[i].GetComponent<Image>().sprite = DoSelecionado;
                else
                    colorButtons[i].GetComponent<Image>().sprite = doPadrao;
            }


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < colorButtons.Length;i++)
        {
            if (colorButtons[i].buttonUp)
            {
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.colorButtonPressed, i));
            }
        }
    }
}
