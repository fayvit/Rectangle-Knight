using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuOrganizadorDeEmblemas
{
    #region inspector
    [SerializeField] private MenuDeEmblemasDisponiveis emblemasD = default;
    [SerializeField] private MenuDeEncaixesDeEmblemas emblemasE = default;
    [SerializeField] private Text numEncaixes = default;
    [SerializeField] private Text infoTitle = default;
    [SerializeField] private Text infoArea = default;
    [SerializeField] private Text custoDeEspacos = default;
    #endregion

    private EstadoDaqui estado = EstadoDaqui.sobreDisponiveis;
    private DadosDoJogador dj;
    private bool estaNoCheckPoint = false;

    private enum EstadoDaqui
    {
        sobreDisponiveis,
        sobreEncaixes,
        negativa
    }

    public void IniciarHud(bool estaNoCheckPoint)
    {
        this.estaNoCheckPoint = estaNoCheckPoint;
        numEncaixes.transform.parent.gameObject.SetActive(true);

        estado = EstadoDaqui.sobreDisponiveis;
        dj = GameController.g.Manager.Dados;

        emblemasE.IniciarHud(EncaixeDeEmblemaSelecionado);
        emblemasD.IniciarHud(EmblemaDisponivelSelecionado);

        emblemasE.RetirarDestaques();

        if (dj.MeusEmblemas.Count > 0)
            ColocaInfoTexts(dj.MeusEmblemas[0]);
        else
        {
            InfoDeNaoTemEmblema();
        }

        numEncaixes.text =  Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas)+" / " + dj.EspacosDeEmblemas;

        EventAgregator.AddListener(EventKey.UiDeEmblemasChange, OnChangeOption);
    }

    void ColocaInfoTexts(Emblema E)
    {
        int indice = (int)E.NomeId;

        infoTitle.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasTitle)[indice];
        infoArea.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasInfo)[indice];

        if (E.EspacosNecessarios > 0)
        {
            custoDeEspacos.text = string.Format(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[6],
                E.EspacosNecessarios.ToString());
        }
        else
            custoDeEspacos.text = "";
    }

    void OnChangeOption(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if ((string)ssge.MyObject[0] == "disponivel")
        {
            if (!(bool)ssge.MyObject[1])
            {
                int I = (int)ssge.MyObject[2];
                ColocaInfoTexts(dj.MeusEmblemas[I]);
            }
            else
            {
                if(dj.MeusEmblemas.Count>0)
                    emblemasD.RetirarDestaques();

                emblemasE.ColocarDestaqueNoSelecionado();
                estado = EstadoDaqui.sobreEncaixes;
                ColocaInfoTexts(Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, emblemasE.OpcaoEscolhida));
            }
        }
        else if ((string)ssge.MyObject[0] == "encaixes")
        {
            if (!(bool)ssge.MyObject[1])
            {
                int I = (int)ssge.MyObject[2];
                ColocaInfoTexts(Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, I));
            }
            else
            {
                estado = EstadoDaqui.sobreDisponiveis;

                emblemasE.RetirarDestaques();

                if (dj.MeusEmblemas.Count > 0)
                {
                    emblemasD.ColocarDestaqueNoSelecionado();
                    ColocaInfoTexts(dj.MeusEmblemas[emblemasD.OpcaoEscolhida]);
                }
                else
                {
                    InfoDeNaoTemEmblema();
                }
            }
        }
    }

    private void InfoDeNaoTemEmblema()
    {
        infoTitle.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[4];
        infoArea.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[5];
        custoDeEspacos.text = "";
    }

    public void FinalizarHud()
    {
        numEncaixes.transform.parent.gameObject.SetActive(false);
        EventAgregator.RemoveListener(EventKey.UiDeEmblemasChange, OnChangeOption);
        emblemasD.FinalizarHud();
        emblemasE.FinalizarHud();
    }

    void EmblemaDisponivelSelecionado(int qual)
    {
        if (estaNoCheckPoint)
        {
            Emblema E = dj.MeusEmblemas[qual];

            if (!E.EstaEquipado)
            {
                if (E.EspacosNecessarios <= dj.EspacosDeEmblemas - Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas))
                {
                    E.EstaEquipado = true;
                    E.OnEquip();
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
                    GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(OnCheckPanel,
                        string.Format(
                        BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[0],
                        E.NomeEmLinguas
                        )
                        );

                    int opcaoGuardada = qual;

                    ReiniciarVisaoDaHud();
                    emblemasD.SelecionarOpcaoEspecifica(opcaoGuardada);

                    ColocaInfoTexts(dj.MeusEmblemas[0]);
                    numEncaixes.text = Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas) + " / " + dj.EspacosDeEmblemas;

                }
                else
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
                    GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(OnCheckPanel,
                        string.Format(
                        BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[1],
                        E.EspacosNecessarios,
                        E.NomeEmLinguas,
                        (dj.EspacosDeEmblemas - Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas)).ToString()));
                }
            }
            else
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
                GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(OnCheckPanel,
                    BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[2]);
            }
        }
        else
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
            EventAgregator.Publish(new SendMethodEvent(EventKey.requestInfoEmblemPanel, OnCheckPanel));
            //painelDeInfoEmblema.ConstroiPainelUmaMensagem(OnCheckPanel);
        }
    }

    void EncaixeDeEmblemaSelecionado(int qual)
    {
        if (estaNoCheckPoint)
        {
            int opcaoGuardada = qual;

            if (Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, opcaoGuardada).NomeId != NomesEmblemas.nulo)
            {

                Emblema E = Emblema.ListaDeEncaixados(dj.MeusEmblemas)[opcaoGuardada];
                E.OnUnequip();
                E.EstaEquipado = false;

                ReiniciarVisaoDaHud();

                emblemasE.SelecionarOpcaoEspecifica(opcaoGuardada);

                ColocaInfoTexts(dj.MeusEmblemas[0]);
                numEncaixes.text = Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas) + " / " + dj.EspacosDeEmblemas;
            }
            else
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
                GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(OnCheckPanel,
                    BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDeEmblema)[3]);
            }
        }
        else
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.triedToChangeEmblemNoSuccessfull));
            EventAgregator.Publish(new SendMethodEvent(EventKey.requestInfoEmblemPanel, OnCheckPanel));
            //painelDeInfoEmblema.ConstroiPainelUmaMensagem(OnCheckPanel);
        }
    }

    public void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.sobreDisponiveis:
                if (dj.MeusEmblemas.Count > 0)
                {
                    emblemasD.MudarOpcao();

                    if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                    {
                        EmblemaDisponivelSelecionado(emblemasD.OpcaoEscolhida);
                    }
                }
                else
                {
                    estado = EstadoDaqui.sobreEncaixes;
                }
            break;
            case EstadoDaqui.sobreEncaixes:
                emblemasE.MudarOpcao();

                if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                {
                    EncaixeDeEmblemaSelecionado(emblemasE.OpcaoEscolhida);
                }

            break;
        }
    }

    void ReiniciarVisaoDaHud()
    {
        emblemasD.FinalizarHud();
        emblemasD.IniciarHud(EmblemaDisponivelSelecionado);
        emblemasE.FinalizarHud();
        emblemasE.IniciarHud(EncaixeDeEmblemaSelecionado);

        emblemasE.RetirarDestaques();
        emblemasD.RetirarDestaques();
    }

    void OnCheckPanel()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestReturnToEmblemMenu));
    }
}
