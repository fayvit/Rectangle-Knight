using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapPanel
{
    [SerializeField] private float vel = 0.1f;
    [SerializeField] private Image imgDoMapa = default;
    [SerializeField] private RectTransform painelDeTamanhoVariavel = default;
    [SerializeField] private ScrollRect sr = default;

    public void IniciarVisualizacaoDoMapa()
    {
        bool foi = false;
        if (imgDoMapa.sprite == null)
            foi = true;
        else if (imgDoMapa.sprite.name != "")
            foi = true;

        painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(true);

        if (foi)
        {
            new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
            {
                new MyInvokeMethod().InvokeAoFimDoQuadro(DesenharMapa);
            });
        }
        
    }

    void DesenharMapa()
    {
        Texture2D tex = GameController.g.MapTexture;


        imgDoMapa.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), tex.texelSize);

        imgDoMapa.rectTransform.localScale = new Vector3(tex.width / 50f, tex.height / 50f, 1);


        float xI = imgDoMapa.rectTransform.sizeDelta.x;
        float yI = imgDoMapa.rectTransform.sizeDelta.y;
        float xP = painelDeTamanhoVariavel.sizeDelta.x;
        float yP = painelDeTamanhoVariavel.sizeDelta.y;

        //Debug.Log("tamanhos: " + xI + " : " + xP + " :" + yI + " : " + yP);

        if (xI > xP || yI > yP)
        {
            painelDeTamanhoVariavel.localScale = new Vector3(tex.width / 50f, tex.height / 50f, 1);
            imgDoMapa.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Update()
    {
        float v = Mathf.Min(CommandReader.GetAxis("vertical", GlobalController.g.Control) + CommandReader.GetAxis("VDpad", GlobalController.g.Control),1);
        float h = Mathf.Min(CommandReader.GetAxis("horizontal", GlobalController.g.Control) + CommandReader.GetAxis("HDpad", GlobalController.g.Control), 1);

        sr.horizontalScrollbar.value += h*vel/painelDeTamanhoVariavel.localScale.x;
        sr.verticalScrollbar.value += v*vel / painelDeTamanhoVariavel.localScale.y;
    }

    public void OnUnpausedGame()
    {
        if (imgDoMapa.sprite.name == "")
        {
            MonoBehaviour.Destroy(imgDoMapa.mainTexture);
            imgDoMapa.sprite = null;
        }
    }

    public void OnExitMapaPanel()
    {
        painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct MyVector2Int
{
    public MyVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x { get; }
    public int y { get; }
}

[System.Serializable]
public struct MyColor
{
    private float r;
    private float g;
    private float b;

    public MyColor(Color C)
    {
        this.r = C.r;
        this.g = C.g;
        this.b = C.b;
    }

    public Color cor { get => new Color(r, g, b); }
}
