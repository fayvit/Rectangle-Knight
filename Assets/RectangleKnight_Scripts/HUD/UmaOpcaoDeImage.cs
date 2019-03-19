using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmaOpcaoDeImage : UmaOpcao
{
    [SerializeField] private Image imgDaOpcao;

    public Image ImgDaOpcao { get => imgDaOpcao; set => imgDaOpcao = value; }

    public void SetarOpcoes(Sprite S)
    {
        imgDaOpcao.sprite = S;
    }
}
