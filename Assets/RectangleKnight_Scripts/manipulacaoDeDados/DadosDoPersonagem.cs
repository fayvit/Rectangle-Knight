using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosDoPersonagem
{

    [SerializeField] private int pontosDeVida = 100;
    [SerializeField] private int maxVida = 100;
    [SerializeField] private int pontosDeMana = 50;
    [SerializeField] private int maxMana = 50;
    [SerializeField] private int ataqueBasico = 25;
    [SerializeField] private int ataqueMagico = 30;

    public virtual int AtaqueMagico { get => ataqueMagico; set => ataqueMagico = value; }
    public virtual int AtaqueBasico { get => ataqueBasico; set => ataqueBasico = value; }

    public int PontosDeVida
    {
        get { return pontosDeVida; }
        set { pontosDeVida = value; }
    }

    public int MaxVida
    {
        get { return maxVida; }
        set { maxVida = value; }
    }

    public int PontosDeMana
    {
        get { return pontosDeMana; }
        set { pontosDeMana = value; }
    }

    public int MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }

    public void ConsomeMana(int valor)
    {
        if (valor > 0)
        {
            if (pontosDeMana - valor >= 0)
                pontosDeMana -= valor;
            else pontosDeMana = 0;
        }
    }

    public void AplicaDano(int valor)
    {
        if (valor > 0)
        {
            if (pontosDeVida - valor >= 0)
                pontosDeVida -= valor;
            else pontosDeVida = 0;
        }
    }

    public void SetarVidaMax()
    {
        pontosDeVida = maxVida;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeLifePoints, PontosDeVida, MaxVida));
    }

    public void SetarManaMax()
    {
        pontosDeMana = maxMana;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMagicPoints, PontosDeMana, MaxMana));
    }

    public void AdicionarVida(int valor)
    {
        if (pontosDeVida + valor <= maxVida)
            pontosDeVida += valor;
        else
            pontosDeVida = maxVida;
    }

    public void AdicionarMana(int valor)
    {
        if (pontosDeMana + valor <= maxMana)
            pontosDeMana += valor;
        else
            pontosDeMana = maxMana;
    }
}