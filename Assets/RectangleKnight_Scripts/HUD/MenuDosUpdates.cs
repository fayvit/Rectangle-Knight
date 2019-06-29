using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuDosUpdates : MenuComInfo
{
    #region inspector
    // [SerializeField] private ImagemDeUpdate[] imgU = default;
    [SerializeField] private VectorOfImagensDeUpdates[] imgUu = default;
    [SerializeField] private Image imgRodape = default;
    #endregion

    private ChaveDeTextoDosUpdates[] Opcoes { get; set; }

    [System.Serializable]
    private struct VectorOfImagensDeUpdates
    {
        [SerializeField] private ImagemDeUpdate[] imgU;

        public ImagemDeUpdate[] ImgU { get => imgU; set => imgU = value; }
    }

    [System.Serializable]
    public struct ImagemDeUpdate
    {
        [SerializeField] private Sprite img;
        [SerializeField] private Sprite rodaPeInfo;
        [SerializeField] private ChaveDeTextoDosUpdates chave;

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
            imgU[indiceDeInteresse].Img, ChangeOption);
    }

    protected override void ChangeOption(int qual)
    {
        int indiceDaMensagem = (int)imgU[(int)Opcoes[qual]].Chave;
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateMenu)[indiceDaMensagem];
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.androidUpdateInfo)[indiceDaMensagem];

        imgRodape.sprite = imgU[(int)Opcoes[qual]].RodaPeInfo;

        if (painelDeTamanhoVariavel.childCount > qual + 1)
        {
            MudarSelecaoParaEspecifico(qual);
        }
    }

    private ImagemDeUpdate[] imgU { get => imgUu[IndexOfControlInfos()].ImgU; }

    private int IndexOfControlInfos()
    {
        int retorno = 0;
        switch (GlobalController.g.Control)
        {
            case Controlador.Android:
                retorno = 0;
            break;
            case Controlador.teclado:
                retorno = 1;
            break;
            default:
                retorno = 0;
            break;
        }

        return retorno;
    }
}