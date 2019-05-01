using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loja : MenuComInfo
{
    [SerializeField] private ItensAVenda[] itensParaVender = default;

    private ItensAVenda[] itensPossiveisDeVender;
    

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        UmaOpcaoComQuantidade uma = G.GetComponent<UmaOpcaoComQuantidade>();
        string titleTxt = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomeParaItensVendidos)[(int)itensPossiveisDeVender[indice].nome];
        string infoTxt = itensPossiveisDeVender[indice].valorDeVenda.ToString();
        uma.SetarOpcao(titleTxt,infoTxt,null,ChangeOption);
    }

    protected override void ChangeOption(int qual)
    {
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.descricaoDosItensVendidos)[(int)itensPossiveisDeVender[qual].nome];
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomeParaItensVendidos)[(int)itensPossiveisDeVender[qual].nome];
    }

    protected override int SetarOpcoes()
    {
        List<ItensAVenda> I = new List<ItensAVenda>();

        for (int i = 0; i < itensParaVender.Length; i++)
        {
            if (GameController.g.MyKeys.VerificaAutoShift(itensParaVender[i].preRequisito) 
                && (itensParaVender[i].quantidadeDisponivel > 0 || itensParaVender[i].quantidadeDisponivel == -1))
            {
                I.Add(itensParaVender[i]);
            }
        }

        itensPossiveisDeVender = I.ToArray();

        ChangeOption(0);
        return I.Count;
    }

    protected override void FinalizarEspecifico()
    {
        TitleUpdate.transform.parent.gameObject.SetActive(false);
        RemoverEventos();
    }

    public bool VerifiqueCompra()
    {
        DadosDoJogador d = GameController.g.Manager.Dados;

        if (d.Dinheiro >= itensPossiveisDeVender[OpcaoEscolhida].valorDeVenda)
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.getCoin, -itensPossiveisDeVender[OpcaoEscolhida].valorDeVenda));
            TradeManager.OnBuy(itensPossiveisDeVender[OpcaoEscolhida].nome);
            itensPossiveisDeVender[OpcaoEscolhida].quantidadeDisponivel--;

            if (itensPossiveisDeVender.Length > 1)
            {
                FinalizarHud();
                IniciarHud();
            }

            return true;
        }
        else
            return false;
    }



}

[System.Serializable]
public class ItensAVenda
{
    public NomeMercadoria nome = NomeMercadoria.nulo;
    public TipoMercadoria tipo = TipoMercadoria.nulo;
    public KeyShift preRequisito = KeyShift.sempretrue;
    public KeyShift keyChange = KeyShift.sempretrue;
    public int quantidadeDisponivel = -1;
    public int valorDeVenda = 500;
}

public enum NomeMercadoria
{
    nulo=-1,
    anelDeIntegridade,
    CQD,
    escadaParaProfundezas,
    SeloPositivistaDoAmor,
    dinheiroMagnetico,//emblema
    suspiroLongo,//Emblema
    fragmentoDeHexagono,
    fragmentoDePentagono
}

public enum TipoMercadoria
{
    nulo=-1,
    inventario,
    emblema,
    itemDeAcao,
    itemDeColecao
}
