using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMoedas {

    public static void Spawn(Vector3 pos, int quantidade)
    {
        if (quantidade < 25)
        {
            InsiraMoedas("moedaPrateada",quantidade,pos);
        }
        else
        {
            if (quantidade % 10 >= 5||quantidade>=50)
            {
                InsiraMoedas("moedaPrateada", quantidade % 10, pos);
                InsiraMoedas("moedaDourada", quantidade/10, pos);
            }
            else
            {
                InsiraMoedas("moedaPrateada", 5, pos);
                InsiraMoedas("moedaPrateada", Mathf.Max((quantidade-5) % 10,0), pos);
                InsiraMoedas("moedaDourada", (quantidade-5) / 10, pos);
            }
        }
    }

    static float GetVariant()
    {
        return Random.Range(-0.5f, 0.5f);
    }

    static void InsiraMoedas(string qual,int quantidade,Vector3 pos)
    {
        GameObject G;
        for (int i = 0; i < quantidade; i++)
        {
            Vector3 dir = pos + GetVariant() * Vector3.right + 0.25f * Vector3.up;
           G = (GameObject)MonoBehaviour.Instantiate(Resources.Load(qual),
                dir,
                Quaternion.identity
                );

             dir = dir - pos;
            dir.Normalize();
            G.GetComponent<Rigidbody2D>().AddForce(dir*8000);
        }
    }
}