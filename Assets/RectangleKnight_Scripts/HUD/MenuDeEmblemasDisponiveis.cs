using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuDeEmblemasDisponiveis : UiDeOpcoes
{

    public void IniciarHud()
    {
        IniciarHUD(25, TipoDeRedimensionamento.emGrade);
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        
    }

    protected override void FinalizarEspecifico()
    {
        
    }

    int LineCellCount()
    {
        GridLayoutGroup grid = painelDeTamanhoVariavel.GetComponent<GridLayoutGroup>();

        return
            (int)(painelDeTamanhoVariavel.rect.width / (grid.cellSize.x + grid.spacing.x));
    }

    int RowCellCount()
    {
        GridLayoutGroup grid = painelDeTamanhoVariavel.GetComponent<GridLayoutGroup>();

        return
            (int)(painelDeTamanhoVariavel.rect.height / (grid.cellSize.y + grid.spacing.y));
    }

    public override void MudarOpcao()
    {
        int quanto = -LineCellCount() * CommandReader.ValorDeGatilhos("VDpad", GameController.g.Manager.Control);

        if (quanto == 0)
            quanto = -LineCellCount() * CommandReader.ValorDeGatilhosTeclado("vertical", GameController.g.Manager.Control);

        if (quanto == 0)
            quanto = CommandReader.ValorDeGatilhos("HDpad", GameController.g.Manager.Control) + CommandReader.ValorDeGatilhos("horizontal", GameController.g.Manager.Control);

        MudarOpcaoComVal(quanto, LineCellCount());
    }
}