using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuDosUpdates : MenuComInfo
{
    #region inspector
    [SerializeField] private ImagemDeUpdate[] imgU = default;
    [SerializeField] private Image imgRodape = default;
    #endregion

    private ChaveDeTextoDosUpdates[] Opcoes { get; set; }

    [System.Serializable]
    private struct ImagemDeUpdate
    {
        [SerializeField]private Sprite img;
        [SerializeField]private Sprite rodaPeInfo;
        [SerializeField]private ChaveDeTextoDosUpdates chave;

        public Sprite Img { get => img; set => img = value; }
        public Sprite RodaPeInfo { get => rodaPeInfo; set => rodaPeInfo = value; }
        public ChaveDeTextoDosUpdates Chave { get => chave; set => chave = value; }
    }

    public override void IniciarHud()
    {
        base.IniciarHud();
    }

    protected override int SetarOpcoes()
    {
        DadosDoJogador J = GameController.g.Manager.Dados;
        bool[] updates = new bool[14]
            {
                true,true,true,true,true,true,J.TemMagicAttack,J.TemDash,J.TemDownArrowJump,J.TemDoubleJump,J.EspadaAzul,
                J.EspadaVerde,J.EspadaDourada,J.EspadaVermelha
            };

        int cont = 0;
        for (int i = 0; i < 14; i++)
        {
            if (updates[i])
            {
                cont++;
            }
        }

        
        Opcoes = new ChaveDeTextoDosUpdates[cont];
        

        int localCont = 0;
        for (int i = 0; i < cont; i++)
        {
            while (!updates[localCont])
            {
                localCont++;
            }

            Opcoes[i] = (ChaveDeTextoDosUpdates)localCont;
            localCont++;
            //Opcoes[i] = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateMenu)[localCont];
        }

        ChangeOption(0);
        return Opcoes.Length;
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        int indiceDeInteresse = (int)Opcoes[indice];
        UmaOpcaoDeUpdates uma = G.GetComponent<UmaOpcaoDeUpdates>();
        uma.SetarOpcao(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateMenu)[indiceDeInteresse],
            imgU[indiceDeInteresse].Img,ChangeOption);
    }

    protected override void ChangeOption(int qual)
    {
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateMenu)[(int)imgU[qual].Chave];
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateInfo)[(int)imgU[qual].Chave];
        imgRodape.sprite = imgU[qual].RodaPeInfo;
    }
}