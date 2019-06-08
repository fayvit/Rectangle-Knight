using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDoBotaoConversaAgendada : AtivadorDoBotaoConversa
{
    [SerializeField] private string ID;

    #region inspector
    [SerializeField] private NpcDeFalasAgendadas esseNpc = default;
    [SerializeField] private KeyShift[] colocarTrue = default;
    [SerializeField] private ColocarTrueCondicional[] colocarTrueCondicional = default;
    [SerializeField] private KeyShift[] condicoesComplementares = default;
    #endregion

    [System.Serializable]
    public struct ColocarTrueCondicional
    {
        public KeyShift condicao;
        public KeyShift alvo;
    }

    // Use this for initialization
    new void Start()
    {
        
        KeyVar myKeys = GameController.g.MyKeys;
        if (!myKeys.VerificaAutoShift(ID))
        {
            for (int i = 0; i < colocarTrue.Length; i++)
            {
                myKeys.MudaShift(colocarTrue[i], true);
            }
        }

        if (colocarTrueCondicional != null)
            for (int i = 0; i < colocarTrueCondicional.Length; i++)
            {
                if (!myKeys.VerificaAutoShift(colocarTrueCondicional[i].condicao))
                    myKeys.MudaShift(colocarTrueCondicional[i].alvo, true);
            }

        myKeys.MudaAutoShift(ID, true);
        myKeys.MudaShift(KeyShift.sempretrue, true);

        npc = esseNpc;
        base.Start();
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    public override void FuncaoDoBotao()
    {
        if(condicoesComplementares!=null)
        for (int i = 0; i < condicoesComplementares.Length; i++)
            GameController.g.MyKeys.MudaShift(condicoesComplementares[i],true);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        base.FuncaoDoBotao();
    }
}