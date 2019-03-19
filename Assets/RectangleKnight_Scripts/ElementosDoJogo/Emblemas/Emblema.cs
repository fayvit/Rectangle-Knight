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
}

public enum NomesEmblemas
{
    nulo,
    dinheiroMagnetico
}
