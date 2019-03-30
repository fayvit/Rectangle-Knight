using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDirection
{
    public static void Flip(Transform T, float dir)
    {
        if (dir != 0)
        {
            Vector3 V = T.localScale;
            T.localScale = new Vector3(-Mathf.Sign(dir) * Mathf.Abs(V.x), V.y, V.z);
        }
    }
}
