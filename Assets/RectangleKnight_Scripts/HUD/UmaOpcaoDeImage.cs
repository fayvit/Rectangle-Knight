using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmaOpcaoDeImage : UmaOpcao
{
    [SerializeField] private Image imgDaOpcao;
    [SerializeField] private Image imgDoEncaixado;

    public Image ImgDaOpcao { get => imgDaOpcao; set => imgDaOpcao = value; }
    public Image ImgDoEncaixado { get => imgDoEncaixado; set => imgDoEncaixado = value; }

    public void SetarOpcoes(Sprite S,System.Action<int> A)
    {
        Acao += A;
        imgDaOpcao.sprite = S;
    }
}
