using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDoBotaoConversaAgendada : AtivadorDoBotaoConversa
{
    [SerializeField] private string ID;
    [SerializeField] private NpcDeFalasAgendadas esseNpc;
    [SerializeField] private KeyShift[] colocarTrue;

    // Use this for initialization
    new void Start()
    {
        
        KeyVar myKeys = GameController.g.MyKeys;
        if (!myKeys.VerificaAutoShift(ID))
            for (int i = 0; i < colocarTrue.Length; i++)
            {
                myKeys.MudaShift(colocarTrue[i], true);
            }
        npc = esseNpc;
        base.Start();
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    public override void FuncaoDoBotao()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        base.FuncaoDoBotao();
    }
}