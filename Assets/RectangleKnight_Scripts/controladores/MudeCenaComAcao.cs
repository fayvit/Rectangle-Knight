using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudeCenaComAcao : AtivadorDeBotao
{

    [SerializeField] private Vector3 posAlvo = default;
    [SerializeField] private NomesCenas[] cenasAlvo = default;
    [SerializeField] private NomesCenas cenaAtiva = NomesCenas.nula;

    private bool ativou = false;

    public override void FuncaoDoBotao()
    {
        if (!ativou)
        {
            ativou = true;
            GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplete);
        }
        
    }

    void OnFadeOutComplete()
    {
        StaticMudeCena.OnFadeOutComplete(cenasAlvo, cenaAtiva, posAlvo);
    }

    // Start is called before the first frame update
    void Start()
    {
        //SempreEstaNoTrigger();
    }


}
