using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LojaDeHerika : Loja
{
    [SerializeField] private HexagonoColetavelBase pHexagono = default;
    [SerializeField] private HexagonoColetavelBase pPentagono = default;

    public override void IniciarHud()
    {
        EventAgregator.AddListener(EventKey.buyUpdateGeometry, OnBuyUpdateGeometry);
        EventAgregator.AddListener(EventKey.hexCloseSecondPanel, OnCloseHexSecondPanel);
        base.IniciarHud();
    }

    protected override void TextosDoNadaParaVender()
    {
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.textosDaLojaDeHerika)[1];
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.textosDaLojaDeHerika)[0];
    }

    protected override void FinalizarEspecifico()
    {
        EventAgregator.RemoveListener(EventKey.buyUpdateGeometry, OnBuyUpdateGeometry);
        EventAgregator.RemoveListener(EventKey.hexCloseSecondPanel, OnCloseHexSecondPanel);
        base.FinalizarEspecifico();
    }

    void OnCloseHexSecondPanel(IGameEvent e)
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.compraConcluida);
        //EventAgregator.Publish(EventKey.musi)
    }

    void OnBuyUpdateGeometry(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        if ((bool)ssge.MyObject[0])
            pPentagono.Coletou(true, "lojaDeHerika");
        else
            pHexagono.Coletou(false, "lojaDeHerika");
    }
}
