using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UmaOpcao : MonoBehaviour
{
    [SerializeField] private Image spriteDoItem;
    private System.Action<int> acao;
    
    public Image SpriteDoItem
    {
        get { return spriteDoItem; }
        set { spriteDoItem = value; }
    }

    protected System.Action<int> Acao
    {
        get { return acao; }
        set { acao = value; }
    }

    public virtual void FuncaoDoBotao()
    {
        Acao(transform.GetSiblingIndex() - 1);
    }
}
