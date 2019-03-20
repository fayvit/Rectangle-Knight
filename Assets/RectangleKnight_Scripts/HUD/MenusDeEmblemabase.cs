using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenusDeEmblemabase : UiDeOpcoes
{
    public void RetirarDestaques()
    {
        UmaOpcao[] umaS = painelDeTamanhoVariavel.GetComponentsInChildren<UmaOpcao>();
        RetirarTodosOsDestaques(umaS);
    }

    public void ColocarDestaqueNoSelecionado()
    {
        UmaOpcao uma = painelDeTamanhoVariavel.transform.GetChild(OpcaoEscolhida + 1).GetComponent<UmaOpcao>();
        ColocarDestaqueNoSelecionado(uma);
    }
}