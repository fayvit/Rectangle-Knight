using UnityEngine;
using UnityEngine.UI;


public class ParentearNaHUD
{

    public static GameObject Parentear(GameObject itemDoContainer, RectTransform containerDeTamanhoVariavel)
    {

        GameObject G = MonoBehaviour.Instantiate(itemDoContainer);
        RectTransform T = G.GetComponent<RectTransform>();
        T.SetParent(containerDeTamanhoVariavel.transform);

        T.localScale = new Vector3(1, 1, 1);

        T.offsetMax = Vector2.zero;
        T.offsetMin = Vector2.zero;


        return G;

    }
}
