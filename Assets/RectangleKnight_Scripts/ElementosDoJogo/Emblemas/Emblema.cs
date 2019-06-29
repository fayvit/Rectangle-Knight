using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Emblema
{
    public NomesEmblemas NomeId { get; private  set; } = NomesEmblemas.nulo;

    public  int EspacosNecessarios { get; private set; } = 1;

    public  bool EstaEquipado { get; set; } = false;

    public Emblema(NomesEmblemas nome, int espacos)
    {
        NomeId = nome;
        EspacosNecessarios = espacos;
    }

    public void OnEquip()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.emblemEquip, NomeId));
    }

    public void OnUnequip()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.emblemUnequip, NomeId));
    }

    public static int NumeroDeEspacosOcupados(List<Emblema> L)
    {
        int cont = 0;
        for (int i = 0; i < L.Count; i++)
        {
            if (L[i].EstaEquipado)
                cont += L[i].EspacosNecessarios;
        }

        return cont;
    }

    public static Emblema VerificarOcupacaoDoEncaixe(List<Emblema> L,int indice)
    {
        List<Emblema> encaixados = ListaDeEncaixados(L);
        if (encaixados.Count > indice)
            return encaixados[indice];
        else
            return new Emblema(NomesEmblemas.nulo,0);
        
    }

    public static List<Emblema> ListaDeEncaixados(List<Emblema> L)
    {
        List<Emblema> encaixados = new List<Emblema>();

        for (int i = 0; i < L.Count; i++)
        {
            if (L[i].EstaEquipado)
            {
                encaixados.Add(L[i]);
            }
        }

        return encaixados;
    }

    public string NomeEmLinguas
    {
        get => BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasTitle)[(int)NomeId];
    }

    public static Emblema GetEmblem(NomesEmblemas nome)
    {
        Emblema retorno = null;
        switch (nome)
        {
            case NomesEmblemas.nulo:
                Debug.LogError("o valor de emblema era nulo");
            break;
            case NomesEmblemas.dinheiroMagnetico:
                retorno = new Emblema(nome,1);
            break;
            case NomesEmblemas.ataqueAprimorado:
                retorno = new Emblema(nome, 2);
            break;
            case NomesEmblemas.suspiroLongo:
                retorno = new Emblema(nome, 1);
            break;
            default:
                Debug.LogError("o valor de emblema não está no switch case");
            break;
        }
        return retorno;
    }
}

public enum NomesEmblemas
{
    nulo,
    dinheiroMagnetico,
    ataqueAprimorado,
    suspiroLongo
}
