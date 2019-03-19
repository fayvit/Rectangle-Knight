using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuOrganizadorDeEmblemas
{
    [SerializeField] private MenuDeEmblemasDisponiveis emblemasD;
    [SerializeField] private MenuDeEncaixesDeEmblemas emblemasE;

    private EstadoDaqui estado = EstadoDaqui.sobreDisponiveis;

    private enum EstadoDaqui
    {
        sobreDisponiveis,
        sobreEncaixes,
        negativa
    }

    public void IniciarHud()
    {
        estado = EstadoDaqui.sobreDisponiveis;

        emblemasD.IniciarHud();
        emblemasE.IniciarHud();
    }

    public void FinalizarHud()
    {
        emblemasD.FinalizarHud();
        emblemasE.FinalizarHud();
    }

    public void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.sobreDisponiveis:
                emblemasD.MudarOpcao();
            break;
        }
    }
}
