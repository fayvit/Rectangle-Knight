using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loja : MenuComInfo
{
    [SerializeField] private ItensAVenda[] itensParaVender = default;
    [SerializeField] public string ID { get; set; }

    private ItensAVenda[] itensPossiveisDeVender;

    public override void IniciarHud()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
        base.IniciarHud();
    }
    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        UmaOpcaoComQuantidade uma = G.GetComponent<UmaOpcaoComQuantidade>();
        string titleTxt = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomeParaItensVendidos)[(int)itensPossiveisDeVender[indice].nome];
        string infoTxt = itensPossiveisDeVender[indice].valorDeVenda.ToString();
        uma.SetarOpcao(titleTxt,infoTxt,
            Resources.Load<Sprite>(itensPossiveisDeVender[indice].nome.ToString())
            ,ChangeOption);
    }

    protected override void ChangeOption(int qual)
    {
        if (itensPossiveisDeVender.Length > 0)
        {
            MudarSelecaoParaEspecifico(qual);
            InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.descricaoDosItensVendidos)[(int)itensPossiveisDeVender[qual].nome];
            TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomeParaItensVendidos)[(int)itensPossiveisDeVender[qual].nome];
        }
        else
        {
            TextosDoNadaParaVender();
        }
    }

    protected virtual void TextosDoNadaParaVender()
    {
        InfoUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.textosDaLojaDeHerika)[1];
        TitleUpdate.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.textosDaLojaDeHerika)[0];
    }

    protected override int SetarOpcoes()
    {
        List<ItensAVenda> I = new List<ItensAVenda>();
        KeyVar myKeys = GameController.g.MyKeys;

        for (int i = 0; i < itensParaVender.Length; i++)
        {
            Debug.Log("Id daqui: " + ID);

            if (myKeys.VerificaAutoShift("concluido, loja " + ID + " item " + i))
            {
                itensParaVender[i].quantidadeDisponivel = 0;
            }
            else if (myKeys.VerificaAutoCont("quantidade disponivel, loja " + ID + " item " + i) > 0)
                itensParaVender[i].quantidadeDisponivel = myKeys.VerificaAutoCont("quantidade disponivel, loja " + ID + " item " + i);
        }

        for (int i = 0; i < itensParaVender.Length; i++)
        {
            if (myKeys.VerificaAutoShift(itensParaVender[i].preRequisito) 
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
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Book1));
        RemoverEventos();
    }

    public bool VerifiqueCompra()
    {
        if (itensPossiveisDeVender.Length > 0)
        {
            DadosDoJogador d = GameController.g.Manager.Dados;

            if (d.Dinheiro >= itensPossiveisDeVender[OpcaoEscolhida].valorDeVenda)
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getCoin, -itensPossiveisDeVender[OpcaoEscolhida].valorDeVenda));
                new MyInvokeMethod().InvokeNoTempoReal(() =>
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Shop));
                }, .5f);
                TradeManager.OnBuy(itensPossiveisDeVender[OpcaoEscolhida].nome);

                // itens possivel de vender vs itens para vender


                itensPossiveisDeVender[OpcaoEscolhida].quantidadeDisponivel--;

                
                KeyVar myKeys = GameController.g.MyKeys;
                int val = itensPossiveisDeVender[OpcaoEscolhida].quantidadeDisponivel;
                int index = (new List<ItensAVenda>(itensParaVender)).IndexOf(itensPossiveisDeVender[OpcaoEscolhida]);

                myKeys.MudaAutoCont("quantidade disponivel, loja " + ID + " item " + index,val);

                if (val == 0)
                    myKeys.MudaAutoShift("concluido, loja " + ID + " item " + index, true);

                FinalizarHud();
                IniciarHud();


                return true;
            }
            else
                return false;
        }
        else
        {
            new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
            {
                EventAgregator.Publish(EventKey.compraConcluida);
            });
            return true;
        }
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
