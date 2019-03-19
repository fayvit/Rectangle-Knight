using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3dsAxis
{

    private static Dictionary<string, float> lerpVal = new Dictionary<string, float>();

    public static int StaticGetAxis(string essGatilho)
    {
        int retorno = 0;
        retorno = Input.GetKey(N3DS_KeysDic.dicAxis[essGatilho].pos)
            ? 1 :
            Input.GetKey(N3DS_KeysDic.dicAxis[essGatilho].neg) ? -1 : 0;
        return retorno;
    }

    public static float GetAxis(string esseGatilho)
    {

        float retorno = 0;
        if (lerpVal.ContainsKey(esseGatilho))
            retorno = StaticGetAxis(esseGatilho);
        else
            lerpVal[esseGatilho] = 0;

        if (retorno != 0)
            lerpVal[esseGatilho] = Mathf.Lerp(lerpVal[esseGatilho], retorno, 5 * Time.fixedDeltaTime);
        else
            lerpVal[esseGatilho] = 0;

        retorno = lerpVal[esseGatilho];
        return retorno;
    }
}
