using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuDoInventario : MenuComInfo
{
    [SerializeField] private Image[] seloPositivista = default;
    [SerializeField] private Text[] quantidadePositivista = default;

    private NomeItem[] Opcoes;

    public override void IniciarHud()
    {
        itemDoContainer.SetActive(false);
        base.IniciarHud();
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        int itemDeInteresse = (int)Opcoes[indice];
       
        Sprite S = Resources.Load<Sprite>(Opcoes[indice].ToString());

        UmaOpcaoComQuantidade uma = G.GetComponent<UmaOpcaoComQuantidade>();
        uma.SetarOpcao(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomesItens)[indice],
            GameController.g.Manager.Dados.QuantidadeNoInventarioDoItem(Opcoes[indice]).ToString(),
            S, ChangeOption);

    }

    protected override void ChangeOption(int qual)
    {
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomesItens)[(int)Opcoes[qual]];
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.descricaoDosItensNoInventario)[(int)Opcoes[qual]];

        if (painelDeTamanhoVariavel.childCount > qual + 1)
        {
            MudarSelecaoParaEspecifico(qual);
        }
    }

    public override void MudarOpcao()
    {
        if(Opcoes.Length>0)
            base.MudarOpcao();
    }

    protected override int SetarOpcoes()
    {
        DadosDoJogador dj = GameController.g.Manager.Dados;
        ItemBase[] nI = dj.MeusItens.ToArray();
        Opcoes = new NomeItem[nI.Length];

        for (int i = 0; i < nI.Length; i++)
        {
            Opcoes[i] = nI[i].Nome;
        }

        for(int i=0;i<3;i++)
        {
            if(dj.NumberedPositivistStamp(i)>0)
            seloPositivista[i].color = Color.white;
            quantidadePositivista[i].text = "x" + dj.NumberedPositivistStamp(i);
        }

        if (Opcoes.Length > 0)
            ChangeOption(0);
        else
        {
            TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.complementosDoMenuDePause)[0];
            InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.complementosDoMenuDePause)[1];
        }
        
        return Opcoes.Length;
    }
}
