using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public NomeItem Nome { get; set; }
    public int Quantidade { get; set; }

    public static void AddItem(DadosDoJogador d,NomeItem n,int quantidade)
    {
        bool foi = false;
        for (int i = 0; i < d.MeusItens.Count; i++)
        {
            if (d.MeusItens[i].Nome == n)
            {
                foi = true;
                d.MeusItens[i].Quantidade += quantidade;
            }
        }

        if (!foi)
        {
            d.MeusItens.Add(new ItemBase() { Nome = n, Quantidade = quantidade });
        }
    }
}

public enum NomeItem
{
    nulo = -1,
    anelDeIntegridade,
    CQD,
    escadaParaGargantaDasProfundezas
}
