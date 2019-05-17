using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemComPainelKey : MensagemComPainel
{
    [SerializeField] private ChaveDeTexto key = default;

    public override void FuncaoDoBotao()
    {
        if (GameController.g.Manager.Estado == EstadoDePersonagem.aPasseio)
        {
            MensagemDeInfo();
            EssePainel.ConstroiPainelUmaMensagem(RetornoDoPainel,BancoDeTextos.RetornaFraseDoIdioma(key));
        }
    }
}
