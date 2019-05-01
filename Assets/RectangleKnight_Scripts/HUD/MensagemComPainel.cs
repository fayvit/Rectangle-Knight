using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemComPainel : AtivadorDeBotao
{
    #region Inspector
    [SerializeField] private PainelUmaMensagem essePainel = default;
    #endregion

    protected virtual void Start()
    {
        SempreEstaNoTrigger();
    }

    public override void FuncaoDoBotao()
    {
        if (GameController.g.Manager.Estado == EstadoDePersonagem.aPasseio)
        {
            Time.timeScale = 0;
            EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic,2.5f));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
            essePainel.ConstroiPainelUmaMensagem(RetornoDoPainel);
        }
    }

    public virtual void RetornoDoPainel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
        EventAgregator.Publish(EventKey.restartMusic, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Book1));
    }
}
