﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackManager
{
#pragma warning disable 0649
    [SerializeField] private GameObject colisorDoAtaqueComum;
    [SerializeField] private GameObject colisorDoAtaquePraCima;
    [SerializeField] private GameObject colisorDoAtaquePrabaixo;
#pragma warning restore 0649
    private int corDeEspadaSelecionada = 0;
    private float tempoDecorrido = 0;
    private bool estouAtacando = false;
    private AttacksTipes tipo = AttacksTipes.neutro;

    private const float tempoDoAtaqueComum = 0.15f;
    private const float tempoDoAtaqueCimaBaixo = 0.25f;
    private const float tempoDoAtaquePraCIma = 0.2f;
    private const float INTERVALO_DE_ATAQUE = .12f;

    private enum AttacksTipes
    {
        neutro = -1,
        comum,
        emAr,
        puloPraBaixo,
        praCima
    }

    public void ChangeSwirdColor(int cor)
    {
        Color C = Color.white;
        corDeEspadaSelecionada = cor;

        switch (cor)
        {
            case 0:
                C = Color.white;
            break;
            case 1:
                C = Color.blue;
            break;
            case 2:
                C = Color.green;
            break;
            case 3:
                C = new Color(0.68f,0.75f,0);
            break;
            case 4:
                C = Color.red;
            break;
        }

        colisorDoAtaqueComum.GetComponent<SpriteRenderer>().color = C;
        colisorDoAtaquePrabaixo.GetComponent<SpriteRenderer>().color = C;
        colisorDoAtaquePraCima.GetComponent<SpriteRenderer>().color = C;

        EventAgregator.Publish(new StandardSendGameEvent(null, EventKey.colorChanged, cor));
    }

    public void ModificouCorDaEspada(int plus,DadosDoJogador dados)
    {
        if (corDeEspadaSelecionada + plus < 5 && corDeEspadaSelecionada + plus > -1)
            corDeEspadaSelecionada += plus;
        else if (corDeEspadaSelecionada + plus >= 5)
            corDeEspadaSelecionada = 0;
        else if (corDeEspadaSelecionada + plus <= -1)
            corDeEspadaSelecionada = 4;

        switch (corDeEspadaSelecionada)
        {
            case 1:
                if (!dados.EspadaAzul)
                    ModificouCorDaEspada(plus, dados);
            break;
            case 2:
                if (!dados.EspadaVerde)
                    ModificouCorDaEspada(plus, dados);
            break;
            case 3:
                if (!dados.EspadaDourada)
                    ModificouCorDaEspada(plus, dados);
            break;
            case 4:
                if (!dados.EspadaVermelha)
                    ModificouCorDaEspada(plus, dados);
            break;
        }

        ChangeSwirdColor(corDeEspadaSelecionada);
    }

    public void AttackIntervalUpdate()
    {
        if(!estouAtacando)
            tempoDecorrido += Time.deltaTime;
    }

    public bool IniciarAtaqueSePodeAtacar()
    {
        if (tempoDecorrido > INTERVALO_DE_ATAQUE)
        {
            estouAtacando = true;
            tempoDecorrido = 0;
            return true;
        }

        return false;
    }

    public void DisparaAtaqueComum()
    {
        if (IniciarAtaqueSePodeAtacar())
        {
            tipo = AttacksTipes.comum;
            colisorDoAtaqueComum.SetActive(true);
        }
        
    }

    public void DisparaAtaquePraCima()
    {
        if (IniciarAtaqueSePodeAtacar())
        {
            tipo = AttacksTipes.praCima;
            colisorDoAtaquePraCima.SetActive(true);
        }
    }

    public void DisparaAtaquePuloPraBaixo()
    {
        if (IniciarAtaqueSePodeAtacar())
        {
            tipo = AttacksTipes.puloPraBaixo;
            colisorDoAtaquePrabaixo.SetActive(true);
        }
    }

    public bool UpdateAttack()
    {
        tempoDecorrido += Time.deltaTime;
        switch (tipo)
        {
            case AttacksTipes.comum:
                if (tempoDecorrido > tempoDoAtaqueComum)
                {
                    ResetaAttackManager();
                    return true;
                }
            break;
            case AttacksTipes.puloPraBaixo:
                if (tempoDecorrido > tempoDoAtaqueCimaBaixo)
                {
                    ResetaAttackManager();
                    return true;
                }
            break;
            case AttacksTipes.praCima:
                if (tempoDecorrido > tempoDoAtaqueCimaBaixo)
                {
                    ResetaAttackManager();
                    return true;
                }
            break;
        }
        return false;
    }

    public void ResetaAttackManager()
    {
        tempoDecorrido = 0;
        estouAtacando = false;
        colisorDoAtaqueComum.SetActive(false);
        colisorDoAtaquePrabaixo.SetActive(false);
        colisorDoAtaquePraCima.SetActive(false);
    }
}