﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController g;

    #region inspector
    [SerializeField] private DisparaTexto dispataT;
    [SerializeField] private PauseMenu pauseM = default;
    [SerializeField] private GameObject sacoDeDinheiro = default;
    #endregion

    private EstadoDoJogo estado = EstadoDoJogo.emGame;

    private enum EstadoDoJogo
    {
        emGame,
        emPause
    }

    public KeyVar MyKeys { get; private set; } = new KeyVar();

    public CharacterManager Manager { get; set; }

    private void Awake()
    {
        if (g == null)
            g = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        GameController[] g = FindObjectsOfType<GameController>();

        if (g.Length > 1)
            Destroy(gameObject);

        transform.parent = null;
        DontDestroyOnLoad(gameObject);


        EventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.AddListener(EventKey.enemyDefeatedCheck, OnEnemyDefeatedCheck);
        EventAgregator.AddListener(EventKey.requestChangeEnemyKey, OnRequestChangeEnemyKey);
        EventAgregator.AddListener(EventKey.destroyShiftCheck, MyDestroyShiftCheck);
        EventAgregator.AddListener(EventKey.requestChangeShiftKey, OnRequestChangeShiftKey);
        EventAgregator.AddListener(EventKey.enterPause, OnEnterPause);
        EventAgregator.AddListener(EventKey.exitPause, OnExitPause);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.RemoveListener(EventKey.enemyDefeatedCheck, OnEnemyDefeatedCheck);
        EventAgregator.RemoveListener(EventKey.requestChangeEnemyKey, OnRequestChangeEnemyKey);
        EventAgregator.RemoveListener(EventKey.destroyShiftCheck, MyDestroyShiftCheck);
        EventAgregator.RemoveListener(EventKey.requestChangeShiftKey, OnRequestChangeShiftKey);
        EventAgregator.RemoveListener(EventKey.enterPause, OnEnterPause);
        EventAgregator.RemoveListener(EventKey.exitPause, OnExitPause);
    }

    private void OnExitPause(IGameEvent obj)
    {
        estado = EstadoDoJogo.emGame;
    }

    private void OnEnterPause(IGameEvent e)
    {
        estado = EstadoDoJogo.emPause;
    }

    private void OnRequestChangeShiftKey(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        MyKeys.MudaAutoShift((string)ssge.MyObject[0], true);
    }

    private void MyDestroyShiftCheck(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        if (MyKeys.VerificaAutoShift((string)ssge.MyObject[0]))
            Destroy((GameObject)ssge.MyObject[1]);
    }

    private void OnRequestChangeEnemyKey(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        MyKeys.MudaEnemyShift((string)ssge.MyObject[0], true);
    }

    private void OnEnemyDefeatedCheck(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        if (MyKeys.VerificaEnemyShift((string)ssge.MyObject[0]))
            Destroy((GameObject)ssge.MyObject[1]);
    }

    private void OnRequestFillDates(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        SaveDates S = (SaveDates)ssge.MyObject[0];

        if (S == null)
        {
            MyKeys = new KeyVar();
        }
        else
        {
            DinheiroCaido d = S.Dados.DinheiroCaido;

            if (d.estaCaido 
                && 
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name==S.VariaveisChave.CenaAtiva.ToString())
            {
               GameObject G = Instantiate(sacoDeDinheiro,MelhoraPos.NovaPos(d.Pos,2.5f),Quaternion.identity);
                G.SetActive(true);
            }
            MyKeys = S.VariaveisChave;
        }
    }

        // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDoJogo.emGame:
                if (CommandReader.ButtonUp(6, GlobalController.g.Control) || CommandReader.ButtonUp(7, GlobalController.g.Control))
                {
                    BtnPauseMenu();
                }
            break;
            case EstadoDoJogo.emPause:
                pauseM.Update();
            break;
        }
    }

    public void BtnPauseMenu()
    {
        pauseM.BtnPauseMenu();
    }

    public void BtnMudarAba(int qual)
    {
        pauseM.BtnTrocarAba(qual);
    }
}
