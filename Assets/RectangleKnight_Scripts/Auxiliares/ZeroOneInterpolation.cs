using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroOneInterpolation
{
    public static float PolinomialInterpolation(float f,float power)
    {
        return Mathf.Pow(f, power);
    }

    public static float OddPolynomialInterpolation(float f, uint power)
    {
        if (power % 2 == 0)
        {
            Debug.LogWarning("A potencia de OddPolynomialInterpolation não era impar, então somamos 1");
            power++;
        }

        return 0.5f*Mathf.Pow(2*f-1,power)+0.5f;
    }

    public static float RadicalOddInterpolation(float f, uint index)
    {
        if (index % 2 == 0)
        {
            Debug.LogWarning("A potencia de RadicalOddInterpolation não era impar, então somamos 1");
            index++;
        }

        if (2 * f - 1 >= 0)
        {
            float cubeRoot = Mathf.Pow(2 * f - 1, 1f / index);
           // Debug.Log(1f/index+" : "+f+" :"+(2*f-1)+" : "+cubeRoot);
            return 0.5f * cubeRoot + 0.5f;
        }
        else
        {
            float cubeRoot = -Mathf.Pow(1-2 * f, 1f / index);
         //   Debug.Log(1f / index + " : " + f + " :" + (1-2 * f ) + " : " + cubeRoot);
            return -0.5f * Mathf.Pow(1 - 2 * f, 1f / index) + 0.5f;
        }
    }

    public static float LagrangeInterppolation(float f, params Vector2[] V)
    {
        float sum = 0;
        for (int i = 0; i < V.Length; i++)
        {
            float prod = 1;
            for (int j = 0; j < V.Length; j++)
            {
                if (i != j)
                    prod *=( (f - V[j].x) / (V[i].x - V[j].x));
            }

            prod *= f*(f-1)/(V[i].x*(V[i].x-1))* V[i].y;

            sum += prod;
        }

        float prod2 = 1;

        for (int j = 0; j < V.Length; j++)
        {
            prod2 *= (f - V[j].x) / (1 - V[j].x);
        }

        sum += prod2*f;

        return sum;
    }

    public static float ParabolaZeroUm(float t)
    {
        return -4 * t * t + 4 * t;
    }

    public static Vector2 ParabolaDeDeslocamento(Vector2 origim,Vector2 target,Vector2 vertice,float t)
    {
        float d = Mathf.Abs(target.x - origim.x);

        float ttt =  d*( origim.x * (1 - t) + t  * (1 + origim.x));

        float tt = ttt / d - origim.x;

        //Debug.Log(t+" : "+ttt + " : " + tt + " : " + ParabolaZeroUm(tt) + " : " + (vertice.y - origim.y));

        return new Vector2(origim.x + t * (-origim.x + target.x),(vertice.y-origim.y)*ParabolaZeroUm(tt)+origim.y);

        
    }
}