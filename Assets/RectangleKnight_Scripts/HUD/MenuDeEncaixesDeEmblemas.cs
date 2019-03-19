using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuDeEncaixesDeEmblemas : UiDeOpcoes
{
    #region inspector
    [SerializeField] private Sprite encaixeLivre = default;
    #endregion

    List<Emblema> emblemasEquipados = new List<Emblema>();

    public void IniciarHud()
    {
        emblemasEquipados = new List<Emblema>();
        DadosDoJogador dados = GameController.g.Manager.Dados;
        int ocupado = 0;
        
        for (int i = 0; i < dados.MeusEmblemas.Count; i++)
        {
            if (dados.MeusEmblemas[i].EstaEquipado)
            {
                ocupado = dados.MeusEmblemas[i].EspacosNecessarios;
                emblemasEquipados.Add(dados.MeusEmblemas[i]);
            }
        }


        IniciarHUD(emblemasEquipados.Count+dados.EspacosDeEmblemas-ocupado, TipoDeRedimensionamento.horizontal);
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        UmaOpcaoDeImage uma = G.GetComponent<UmaOpcaoDeImage>();

        if (indice < emblemasEquipados.Count)
        {
            uma.SetarOpcoes(default);
        }
        else
            uma.SetarOpcoes(encaixeLivre);
    }

    protected override void FinalizarEspecifico()
    {
        
    }
}
