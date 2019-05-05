using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuDeEncaixesDeEmblemas : MenusDeEmblemabase
{
    #region inspector
    [SerializeField] private Sprite encaixeLivre = default;
    #endregion

    private System.Action<int> Acao;
    private List<Emblema> emblemasEquipados = new List<Emblema>();

    public void IniciarHud(System.Action<int> acaoStandard)
    {
        Acao += acaoStandard;
        emblemasEquipados = new List<Emblema>();
        DadosDoJogador dados = GameController.g.Manager.Dados;
        int ocupado = 0;
        
        for (int i = 0; i < dados.MeusEmblemas.Count; i++)
        {
            if (dados.MeusEmblemas[i].EstaEquipado)
            {
                ocupado += dados.MeusEmblemas[i].EspacosNecessarios;
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
            Emblema E = emblemasEquipados[indice];
           // Texture2D t2d = (Texture2D)Resources.Load(E.NomeId.ToString());
            Sprite S = Resources.Load<Sprite>(E.NomeId.ToString());//Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), t2d.texelSize);

            uma.SetarOpcoes(S,Acao);
            
        }
        else
            uma.SetarOpcoes(encaixeLivre,Acao);
    }

    public override void MudarOpcao()
    {
        int quanto = CommandReader.ValorDeGatilhos("VDpad", GameController.g.Manager.Control);

        if (quanto == 0)
            quanto = CommandReader.ValorDeGatilhosTeclado("vertical", GameController.g.Manager.Control);

        bool mudou = quanto!=0;

        int opcaoGuardada = OpcaoEscolhida;
        base.MudarOpcao_H(true);

        if(opcaoGuardada!=OpcaoEscolhida|| mudou)
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.UiDeEmblemasChange, "encaixes", mudou, OpcaoEscolhida));
    }

    protected override void FinalizarEspecifico()
    {
        Acao = null;
    }
}
