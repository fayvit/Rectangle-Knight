using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UmaOpcaoDeMenu : UmaOpcao
{
    [SerializeField] private Text textoOpcao;

    protected Text TextoOpcao
    {
        get { return textoOpcao; }
        set { textoOpcao = value; }
    }

    public virtual void SetarOpcao(System.Action<int> acaoDaOpcao,string txtDaOpcao)
    {
        Acao += acaoDaOpcao;
        TextoOpcao.text = txtDaOpcao;
    }   
}
