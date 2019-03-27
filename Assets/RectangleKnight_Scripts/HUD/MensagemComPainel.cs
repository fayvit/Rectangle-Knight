using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemComPainel : AtivadorDeBotao
{
    #region Inspector
    [SerializeField] private PainelUmaMensagem essePainel = default;
    #endregion

    private void Start()
    {
        SempreEstaNoTrigger();
    }
    public override void FuncaoDoBotao()
    {
        if (GameController.g.Manager.Estado == EstadoDePersonagem.aPasseio)
        {
            Time.timeScale = 0;
            EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
            essePainel.ConstroiPainelUmaMensagem(RetornoDoPainel);
        }
    }

    public void RetornoDoPainel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
    }
}
