using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuComInfo : UiDeOpcoes
{
    [SerializeField] private Text titleUpdate = default;
    [SerializeField] private Text infoUpdate = default;

    public Text TitleUpdate { get => titleUpdate; set => titleUpdate = value; }
    public Text InfoUpdate { get => infoUpdate; set => infoUpdate = value; }

    protected abstract int SetarOpcoes();
    protected abstract void ChangeOption(int qual);

    public virtual void IniciarHud()
    {
        TitleUpdate.transform.parent.gameObject.SetActive(true);
        int quantidade = SetarOpcoes();

        if (quantidade > 0)
            IniciarHUD(quantidade, TipoDeRedimensionamento.vertical);
        else
            itemDoContainer.SetActive(false);

        EventAgregator.AddListener(EventKey.UiDeOpcoesChange, OnChangeOption);
    }

    void OnChangeOption(IGameEvent e)
    {
        ChangeOption(OpcaoEscolhida);
    }

    protected override void FinalizarEspecifico()
    {
        TitleUpdate.transform.parent.gameObject.SetActive(false);
        RemoverEventos();
    }

    protected void RemoverEventos()
    {
        EventAgregator.RemoveListener(EventKey.UiDeOpcoesChange, OnChangeOption);
    }


}
