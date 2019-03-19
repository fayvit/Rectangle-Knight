using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedimensionarUI
{
    public static void NaVertical(RectTransform redimensionado, GameObject item, int num)
    {
        redimensionado.sizeDelta
            = new Vector2(0, num * item.GetComponent<LayoutElement>().preferredHeight);
    }

    public static void NaHorizontal(RectTransform redimensionado, GameObject item, int num)
    {
        redimensionado.sizeDelta
            = new Vector2( num * item.GetComponent<LayoutElement>().preferredWidth,0);
    }

    public static void EmGrade(RectTransform redimensionado, GameObject item, int num)
    {
        LayoutElement lay = item.GetComponent<LayoutElement>();
        GridLayoutGroup grid = redimensionado.GetComponent<GridLayoutGroup>();
        
        int quantidade = Mathf.CeilToInt(redimensionado.rect.width / (lay.preferredWidth + grid.spacing.x+5));

        Debug.Log("Redimensionar grade: " + num + " :" + quantidade + ": " + (lay.preferredHeight + grid.spacing.y) + " : " + redimensionado.rect.width);
        int numeroDeLinhas = Mathf.CeilToInt((float)(num) / (quantidade));
        redimensionado.sizeDelta
                    = new Vector2(0, numeroDeLinhas * (lay.preferredHeight+ grid.spacing.y));
    }
}

public enum TipoDeRedimensionamento
{
    vertical,
    emGrade,
    horizontal
}