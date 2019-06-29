using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuDoNovoJogoCarregarDeletar : UiDeOpcoes
{
    private int indiceTransitorio;
    private List<PropriedadesDeSave> essaLista;

    public void IniciarHud()
    {
        int quantidade = SaveDatesManager.s.SaveProps.Count;

        essaLista = SaveDatesManager.s.SaveProps;
        essaLista.Sort();

        sr.transform.parent.gameObject.SetActive(true);
        IniciarHUD(quantidade);

        RedimensionarUI.NaVertical(painelDeTamanhoVariavel, itemDoContainer, quantidade+1);

        UmaOpcaoDoIniciarJogo[] umaS = painelDeTamanhoVariavel.GetComponentsInChildren<UmaOpcaoDoIniciarJogo>();

        for (int i = 0; i < umaS.Length; i++)
        {
            umaS[i].ColocarTotalAlpha();
        }
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        UmaOpcaoDoIniciarJogo uma = G.GetComponent<UmaOpcaoDoIniciarJogo>();
        uma.SetarOpcao(ParaCarregar,ParaDeletar, essaLista[indice].nome);
    }

    public void IsDeleteOrLoad()
    {
        if (painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1)
                .GetComponent<UmaOpcaoDoIniciarJogo>().SpriteDoItem.color.a < 1)
        {
            DeletarJogo(OpcaoEscolhida-1);
        }
        else
        {
            IniciarJogo(OpcaoEscolhida-1);
        }
    }

    void ParaCarregar(int qual)
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.startLoadDeleteButtonPress, qual, true));
    }

    void ParaDeletar(int qual)
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.startLoadDeleteButtonPress, qual, false));
    }
    

    public void IniciarJogo(int qual)
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic,2f));
        SaveDatesManager.CarregaSaveDates(qual);

        GlobalController.g.FadeV.IniciarFadeOutComAction(() =>
        {
            SceneLoader.IniciarCarregamento(SaveDatesManager.s.SaveProps[qual].indiceDoSave);
            GlobalController.g.FadeV.IniciarFadeIn();
        });
        
    }

    public void DeletarJogo(int qual)
    {
        string arg = string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.certezaDeletarJogo),
            painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1)
                .GetComponent<UmaOpcaoDoIniciarJogo>().TxtDaqui
            );

        indiceTransitorio = qual;

        GlobalController.g.Confirmacao.AtivarPainelDeConfirmacao(SimQueroDeletar, DesistiDeDeletar, arg);
        
    }

    void SimQueroDeletar()
    {
        /*
        PropriedadesDeSave p = SaveDatesManager.s.SaveProps[indiceTransitorio];

        List<PropriedadesDeSave> lista = SaveDatesManager.s.SaveProps;

        lista.Remove(p);

        SaveDatesManager.Save();

        lista.Sort();*/

        SaveDatesManager.s.RemoveSaveDates(indiceTransitorio);
        
        FinalizarHud(2);
        IniciarHud();

        EventAgregator.Publish(EventKey.returnOfdeleteFile, null);

        
    }

    void DesistiDeDeletar()
    {
        EventAgregator.Publish(EventKey.returnOfdeleteFile, null);
    }

    protected override void FinalizarEspecifico()
    {
        sr.transform.parent.gameObject.SetActive(false);
        
    }

    public override void MudarOpcao()
    {
        int quanto = VerificaMudarOpcao(false);

        if (quanto != 0 && OpcaoEscolhida!=0)
        {

            painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1)
                .GetComponent<UmaOpcaoDoIniciarJogo>().MudarOpcaoLocal();
        }

        quanto = VerificaMudarOpcao(true);
        bool eraDelete = false;

        if (quanto != 0 && OpcaoEscolhida != 0)
        {
            UmaOpcaoDoIniciarJogo uma =
            painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1)
                .GetComponent<UmaOpcaoDoIniciarJogo>();

            if (OpcaoEscolhida != 0)
                eraDelete = uma.SpriteDoItem.color.a == 0;

            uma.ColocarTotalAlpha();
        }

        MudarOpcaoComVal(quanto);

        if (eraDelete&&OpcaoEscolhida!=0)
        {
            painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1)
               .GetComponent<UmaOpcaoDoIniciarJogo>().MudarOpcaoLocal();
        }
    }
}
