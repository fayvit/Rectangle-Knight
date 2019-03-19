using UnityEngine;
using UnityEngine.UI;
using System;

public class UmaOpcaoDeLinguagem : UmaOpcaoDeMenu
{
#pragma warning disable 0649
    [SerializeField] private Image imgDaBandeira;
#pragma warning restore 0649
    public void SetarOpcao(Action<int> acaoDaOpcao, string txtDaOpcao, Sprite img)
    {
        base.SetarOpcao(acaoDaOpcao, txtDaOpcao);
        imgDaBandeira.sprite = img;
    }
}
