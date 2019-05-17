using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciaLigando
{
    public static GameObject Instantiate(GameObject G, Vector3 pos, float tempoDaDestruicao = -1, Quaternion rotation = default)
    {
        GameObject G2 = null;
        if (G != null)
        {
            G2 = MonoBehaviour.Instantiate(G, pos, rotation);
            G2.SetActive(true);
            if (tempoDaDestruicao > 0)
                MonoBehaviour.Destroy(G2, tempoDaDestruicao);
        }
        return G2;
    }
}