using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmaOpcaoComQuantidade : UmaOpcaoDeUpdates
{
    [SerializeField] private Text txtMenor;

    public Text TxtMenor { get => txtMenor; set => txtMenor = value; }

    public void SetarOpcao(string texto, string textoDois,Sprite img, System.Action<int> acao)
    {
        txtMenor.text = textoDois;
        SetarOpcao(texto, img, acao);
    }
}
