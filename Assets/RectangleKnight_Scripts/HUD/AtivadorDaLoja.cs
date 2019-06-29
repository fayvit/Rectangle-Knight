using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDaLoja : AtivadorDeBotao
{
    [SerializeField] public string ID;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    protected Loja EssaLoja { get; set; }

    private enum EstadoDaqui
    {
        emEspera,
        mudandoOpcao,
        mensagemSuspensa

    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    public override void FuncaoDoBotao()
    {
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(EventKey.requestHideControllers, null);
        EssaLoja.ID = ID;
        EssaLoja.IniciarHud();        
        estado = EstadoDaqui.mudandoOpcao;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        EventAgregator.AddListener(EventKey.compraConcluida, OnBuyFinish);
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.compraConcluida, OnBuyFinish);
    }

    void OnBuyFinish(IGameEvent e)
    {
        RetornoDeMensagem();
    }

    protected override void Update()
    {
        base.Update();

        switch (estado)
        {
            case EstadoDaqui.mudandoOpcao:
                Controlador c = GlobalController.g.Control;
                EssaLoja.MudarOpcao();

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
        if (estado==EstadoDaqui.mudandoOpcao)
        {
            if (EssaLoja.VerifiqueCompra())
            {

            } else
            {
                GlobalController.g.UmaMensagem.ConstroiPainelUmaMensagem(RetornoDeMensagem, "Você não tem dinheiro suficiente");
            }

            estado = EstadoDaqui.mensagemSuspensa;
        }
    }

    public void BtnVoltar()
    {
        EssaLoja.FinalizarHud();
        estado = EstadoDaqui.emEspera;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        EventAgregator.Publish(EventKey.requestShowControllers, null);
    }
}
