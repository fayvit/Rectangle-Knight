using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulaDeTransporte : AtivadorDeBotao
{
    #region inspector
    [SerializeField] private MenuBasico menu = default;
    [SerializeField] private GameObject menuContainer = default;
    [SerializeField] private CapsuleID minhaID = CapsuleID.gargantaDasProfundezas;
    #endregion

    private CapsuleInfo infoSend;
    private KeyVar myKeys;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        menuAberto
    }

    public override void FuncaoDoBotao()
    {
        estado = EstadoDaqui.menuAberto;
        menuContainer.SetActive(true);
        myKeys.ListaDeCapsulas.ChangeForActive(minhaID);
        menu.IniciarHud(EscolhaDeViagem, myKeys.ListaDeCapsulas.GetActiveCapsuleNames());
        SaveDatesManager.SalvarAtualizandoDados();
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
    }

    void FakeFadeOut()
    {
        BtnCancelar();
        GlobalController.g.FadeV.IniciarFadeInComAction(FakeFadeIn);
    }

    void FakeFadeIn()
    {
        Time.timeScale = 1;
    }

    void OnFadeOut()
    {
        BtnCancelar();
        StaticMudeCena.OnFadeOutComplete(new NomesCenas[1] { infoSend.Cena }, infoSend.Cena, infoSend.Pos);
        
    }

    private void EscolhaDeViagem(int qual)
    {
        estado = EstadoDaqui.emEspera;

        CapsuleInfo cI = myKeys.ListaDeCapsulas.GetActiveCapsules()[qual];

        if (StaticMudeCena.EstaCenaEstaCarregada(cI.Cena))
        {
            
            GlobalController.g.FadeV.IniciarFadeOutComAction(FakeFadeOut);

            Time.timeScale = 0;
        }
        else
        {
            infoSend = cI;
            GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOut);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(ExistenciaDoController.AgendaExiste(Start,this))
            myKeys = GameController.g.MyKeys;   
    }

    protected override void Update()
    {
        if((myKeys.VerificaAutoShift("-44366_capsulaDaGargantaDasProfundezas")// esse é o key do Otto, ficar atento a modificações
            &&
            !myKeys.VerificaAutoShift(KeyShift.fascinadoPelasCapsulas)) || GlobalController.g.EmTeste)
            base.Update();

        switch (estado)
        {
            case EstadoDaqui.menuAberto:
                menu.MudarOpcao();

                if (ActionManager.ButtonUp(1, GlobalController.g.Control))
                {
                    BtnCancelar();
                }
                else if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                {
                    EscolhaDeViagem(menu.OpcaoEscolhida);
                }
            break;

        }
    }

    public void BtnCancelar()
    {
        estado = EstadoDaqui.emEspera;
        menu.FinalizarHud();
        menuContainer.SetActive(false);
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
    }
}
