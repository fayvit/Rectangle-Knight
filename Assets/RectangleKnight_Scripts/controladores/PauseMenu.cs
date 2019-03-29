﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PauseMenu
{
    #region Inspector
    [SerializeField] private GameObject containerDoMenuDePause = default;
    [SerializeField] private MenuBasico menuDePauseBasico = default;
    [SerializeField] private MenuDeOpcoes menuO = default;
    [SerializeField] private MenuDosUpdates menuU = default;
    [SerializeField] private MenuOrganizadorDeEmblemas menuOE = default;
    [SerializeField] private MenuPentagonosHexagonos pentagonosHexagonos = default;
    [SerializeField] private Image[] abas = default;
    [SerializeField] private Sprite destaque = default;
    [SerializeField] private Sprite padrao = default;
    #endregion

    private int qualMenu = 0;
    private EstadoDePersonagem estadoAoPausar;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        configuracoesAberto,
        updatesAberto,
        emblemasAberto,
        mapaAberto,
        menuDeOpcoesAberto,
        menuSuspensoAberto,
        pentagonosHexagonosAberto
    }

    public void IniciarMenuDePause()
    {
        if (GameController.g.Manager.Estado != EstadoDePersonagem.derrotado)
        {
            Time.timeScale = 0;
            estadoAoPausar = GameController.g.Manager.Estado;
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.enterPause));
            containerDoMenuDePause.SetActive(true);
            IniciarQualMenu(qualMenu);

            EventAgregator.AddListener(EventKey.returnToMainMenu, OnReturnToMainMenu);
            EventAgregator.AddListener(EventKey.triedToChangeEmblemNoSuccessfull, OnTriedEmblem);
            EventAgregator.AddListener(EventKey.requestReturnToEmblemMenu, OnRequestEmblemMenu);
        }
    }

    private void OnRequestEmblemMenu(IGameEvent e)
    {
        estado = EstadoDaqui.emblemasAberto;
    }

    private void OnTriedEmblem(IGameEvent e)
    {
        estado = EstadoDaqui.menuSuspensoAberto;
    }

    private void OnReturnToMainMenu(IGameEvent obj)
    {
        estado = EstadoDaqui.configuracoesAberto;
    }

    void OnBasicPauseOptionSelect(int option)
    {
        switch (option)
        {
            case 0:
                FinalizarMenuDepause();
            break;
            case 1:
                estado = EstadoDaqui.menuDeOpcoesAberto;
                menuO.gameObject.SetActive(true);
            break;
            case 2:
                GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplete);
            break;
        }
    }

    void OnFadeOutComplete()
    {
        Time.timeScale = 1;
        FinalizarMenuDepause();
        SceneLoader.CarergarMenuPrincipal();
    }

    void VerificaMudarAba()
    {
        Controlador Control = GlobalController.g.Control;
        if (estado != EstadoDaqui.menuDeOpcoesAberto && estado !=EstadoDaqui.menuSuspensoAberto)
        {
            int qualSelecionado = qualMenu;
            if (CommandReader.ButtonDown(4, Control))
            {
                qualSelecionado = ContadorCiclico.AlteraContador(-1, qualSelecionado, 5);
            }
            else if (CommandReader.ButtonDown(5, Control))
                qualSelecionado = ContadorCiclico.AlteraContador(1, qualSelecionado, 5);

            if (qualSelecionado != qualMenu)
                BtnTrocarAba(qualSelecionado);
        }
    }

    void VerificarSaidaDoPause()
    {
        
        if (ActionManager.ButtonUp(6, GlobalController.g.Control) || ActionManager.ButtonUp(7, GlobalController.g.Control))
        {
            BtnPauseMenu();
        }
        

    }

    void FinalizarMenuDepause()
    {
        estado = EstadoDaqui.emEspera;
        FinalizarTodasAsAbas();
        Time.timeScale = 1;
        containerDoMenuDePause.SetActive(false);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.exitPause,estadoAoPausar));

        EventAgregator.RemoveListener(EventKey.returnToMainMenu, OnReturnToMainMenu);
        EventAgregator.RemoveListener(EventKey.triedToChangeEmblemNoSuccessfull, OnTriedEmblem);
        EventAgregator.RemoveListener(EventKey.requestReturnToEmblemMenu, OnRequestEmblemMenu);
    }

    public void BtnPauseMenu()
    {
        if (estado == EstadoDaqui.emEspera)
            IniciarMenuDePause();
        else if (estado != EstadoDaqui.menuDeOpcoesAberto && estado != EstadoDaqui.menuSuspensoAberto)
            FinalizarMenuDepause();
    }

    public void BtnTrocarAba(int qual)
    {
        if (estado != EstadoDaqui.menuDeOpcoesAberto && qual!=qualMenu)
        {
            FinalizarTodasAsAbas();
            DestacarAba(qual);
            qualMenu = qual;
            IniciarQualMenu(qual);
        }
    }

    void DestacarAba(int qual)
    {
        for (int i = 0; i < abas.Length;i++)
        {
            if (qual == i)
                abas[i].sprite = destaque;
            else
                abas[i].sprite = padrao;
        }
    }

    void FinalizarTodasAsAbas()
    {
        menuDePauseBasico.FinalizarHud();
        menuU.FinalizarHud();
        menuOE.FinalizarHud();
        pentagonosHexagonos.FinalizarHud();
    }

    void IniciarQualMenu(int qual)
    {
        switch (qual)
        {
            case 0:
                estado = EstadoDaqui.configuracoesAberto;
                menuDePauseBasico.IniciarHud(OnBasicPauseOptionSelect, BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.menuDePause).ToArray());
            break;
            case 1:
                menuU.IniciarHud();
                estado = EstadoDaqui.updatesAberto;
            break;
            case 2:
                menuOE.IniciarHud(estadoAoPausar == EstadoDePersonagem.inCheckPoint);
                estado = EstadoDaqui.emblemasAberto;
            break;
            case 3:
                pentagonosHexagonos.IniciarHud();
                estado = EstadoDaqui.pentagonosHexagonosAberto;
            break;
        }
    }

    public void Update()
    {
        VerificaMudarAba();
        VerificarSaidaDoPause();

        switch (estado)
        {
            case EstadoDaqui.configuracoesAberto:
                #region configuracoesAberto
                menuDePauseBasico.MudarOpcao();

                if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                {
                    OnBasicPauseOptionSelect(menuDePauseBasico.OpcaoEscolhida);
                }
                #endregion
            break;
            
            case EstadoDaqui.updatesAberto:
                menuU.MudarOpcao();
            break;
            case EstadoDaqui.emblemasAberto:
                menuOE.Update();
            break;
        }
    }
}
