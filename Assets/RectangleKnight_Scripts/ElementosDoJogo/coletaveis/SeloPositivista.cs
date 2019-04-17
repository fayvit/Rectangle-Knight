using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeloPositivista : MensagemComPainel
{
    [SerializeField] private TipoSelo tipo = TipoSelo.progresso;
    //[SerializeField] private PainelUmaMensagem umaMensagem;

    public enum TipoSelo
    {
        progresso,
        amor,
        ordem
    }

    public override void FuncaoDoBotao()
    {
        base.FuncaoDoBotao();

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.getStamp, tipo));
    }

    public override void RetornoDoPainel()
    {
        base.RetornoDoPainel();
        Destroy(gameObject);
    }

}
