using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemComPainel : AtivadorDeBotao
{
    #region Inspector
    [SerializeField] private PainelUmaMensagem essePainel = default;
    #endregion

    public PainelUmaMensagem EssePainel { get => essePainel; set => essePainel = value; }

    protected virtual void Start()
    {
        SempreEstaNoTrigger();
    }

    protected void MensagemDeInfo()
    {
        Time.timeScale = 0;
        EventAgregator.Publish(EventKey.abriuPainelSuspenso);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic, 2.5f));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestHideControllers));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
    }

    public override void FuncaoDoBotao()
    {
        if (GameController.g.Manager.Estado == EstadoDePersonagem.aPasseio)
        {
            MensagemDeInfo();
            EssePainel.ConstroiPainelUmaMensagem(RetornoDoPainel);
        }
    }

    public virtual void RetornoDoPainel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        EventAgregator.Publish(EventKey.restartMusic, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Book1));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShowControllers));
    }
}
