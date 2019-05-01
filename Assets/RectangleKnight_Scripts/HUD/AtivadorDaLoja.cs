using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDaLoja : AtivadorDeBotao
{
    [SerializeField] private Loja essaLoja = default;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        mudandoOpcao,
        mensagemSuspensa

    }

    public override void FuncaoDoBotao()
    {
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(EventKey.requestHideControllers, null);
        essaLoja.IniciarHud();
        estado = EstadoDaqui.mudandoOpcao;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        switch (estado)
        {
            case EstadoDaqui.mudandoOpcao:
                Controlador c = GlobalController.g.Control;
                essaLoja.MudarOpcao();

                if (ActionManager.ButtonUp(0, c))
                {
                    BtnComprar();
                } else if (ActionManager.ButtonUp(1, c) || ActionManager.ButtonUp(2, c))
                {
                    BtnVoltar();
                }
            break;
        }
    }

    void RetornoDeMensagem()
    {
        estado = EstadoDaqui.mudandoOpcao;
    }

    public void BtnComprar()
    {

        if (essaLoja.VerifiqueCompra())
        {

        }else
        {
            estado = EstadoDaqui.mensagemSuspensa;
            GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(RetornoDeMensagem,"Você não tem dinheiro suficiente");
        }
    }

    public void BtnVoltar()
    {
        essaLoja.FinalizarHud();
        estado = EstadoDaqui.emEspera;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
    }
}
