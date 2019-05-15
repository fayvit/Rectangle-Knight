using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeloPositivista : MensagemComPainel
{
    [SerializeField] private TipoSelo tipo = TipoSelo.progresso;
    [SerializeField] private string ID = "";
    //[SerializeField] private PainelUmaMensagem umaMensagem;

    public enum TipoSelo
    {
        progresso,
        amor,
        ordem
    }

    protected override void Start()
    {
        base.Start();
        ActiveFalseForShift.StaticStart(Start, this, ID);
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    public override void FuncaoDoBotao()
    {
        base.FuncaoDoBotao();

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.getStamp, tipo));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
    }

    public override void RetornoDoPainel()
    {
        base.RetornoDoPainel();
        Destroy(gameObject);
    }

}
