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
    [SerializeField] private PainelUmaMensagem painelDeInfoEmblema = default;
    [SerializeField] private Text numEncaixes = default;
    [SerializeField] private Text infoTitle = default;
    [SerializeField] private Text infoArea = default;
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

        ColocaInfoTexts((int)dj.MeusEmblemas[0].NomeId);
        numEncaixes.text =  Emblema.NumeroDeEspacosOcupados(dj.MeusEmblemas)+" / " + dj.EspacosDeEmblemas;

        EventAgregator.AddListener(EventKey.UiDeEmblemasChange, OnChangeOption);
    }

    void ColocaInfoTexts(int indice)
    {
        infoTitle.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasTitle)[indice];
        infoArea.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasInfo)[indice];
    }

    void OnChangeOption(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if ((string)ssge.MyObject[0] == "disponivel")
        {
            if (!(bool)ssge.MyObject[1])
            {
                int I = (int)ssge.MyObject[2];
                ColocaInfoTexts((int)dj.MeusEmblemas[I].NomeId);
            }
            else
            {
                emblemasD.RetirarDestaques();
                emblemasE.ColocarDestaqueNoSelecionado();
                estado = EstadoDaqui.sobreEncaixes;
                ColocaInfoTexts((int)Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, emblemasE.OpcaoEscolhida));
            }
        }
        else if ((string)ssge.MyObject[0] == "encaixes")
        {
            if (!(bool)ssge.MyObject[1])
            {
                int I = (int)ssge.MyObject[2];
                I = (int)Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, I);
                ColocaInfoTexts(I);
            }
            else
            {
                emblemasE.RetirarDestaques();
                emblemasD.ColocarDestaqueNoSelecionado();
                estado = EstadoDaqui.sobreDisponiveis;
                ColocaInfoTexts((int)dj.MeusEmblemas[emblemasD.OpcaoEscolhida].NomeId);
            }
        }
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

                    ColocaInfoTexts((int)dj.MeusEmblemas[0].NomeId);
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
            painelDeInfoEmblema.ConstroiPainelUmaMensagem(OnCheckPanel);
        }
    }

    void EncaixeDeEmblemaSelecionado(int qual)
    {
        if (estaNoCheckPoint)
        {
            int opcaoGuardada = qual;

            if (Emblema.VerificarOcupacaoDoEncaixe(dj.MeusEmblemas, opcaoGuardada) != NomesEmblemas.nulo)
            {

                Emblema E = Emblema.ListaDeEncaixados(dj.MeusEmblemas)[opcaoGuardada];
                E.OnUnequip();
                E.EstaEquipado = false;

                ReiniciarVisaoDaHud();

                emblemasE.SelecionarOpcaoEspecifica(opcaoGuardada);

                ColocaInfoTexts((int)dj.MeusEmblemas[0].NomeId);
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
            painelDeInfoEmblema.ConstroiPainelUmaMensagem(OnCheckPanel);
        }
    }

    public void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.sobreDisponiveis:
                emblemasD.MudarOpcao();

                if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                {
                    EmblemaDisponivelSelecionado(emblemasD.OpcaoEscolhida);
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
