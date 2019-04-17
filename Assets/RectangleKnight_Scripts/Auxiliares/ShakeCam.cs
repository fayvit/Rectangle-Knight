using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeCam
{
    
    [SerializeField] private bool testShake;


    private EstadoComplementarDaCamera estadoC = EstadoComplementarDaCamera.estavel;
    private ShakeAxis axis = ShakeAxis.z;
    private Transform transform;
    private float tempoDecorrido = 0;
    private float tempoDeShake = 0.1f;
    private float shakeAngle = 1;
    private int contShake = 0;
    private int totalShake = 5;
    private bool sinal = false;

    public enum EstadoComplementarDaCamera
    {
        shake,
        estabilizando,
        estavel
    }

    public void Construir(Transform transform)
    {
        this.transform = transform;
    }

    public void IniciarShake(ShakeAxis S =ShakeAxis.y, int totalShake = 5, float shakeAngle = 1)
    {
        transform.rotation = Quaternion.identity;

        this.totalShake = totalShake;
        this.shakeAngle = shakeAngle;
        this.axis = S;
        tempoDecorrido = 0;
        contShake = 0;
        estadoC = EstadoComplementarDaCamera.shake;
    }

    public void Update()
    {
        if (testShake)
        {
            IniciarShake();
            testShake = false;
        }

        switch (estadoC)
        {
            case EstadoComplementarDaCamera.shake:
                tempoDecorrido += Time.deltaTime;
                if (contShake < totalShake)
                {
                    transform.localEulerAngles = ConditionalShake(transform);

                    if (tempoDecorrido > tempoDeShake)
                    {
                        tempoDecorrido = 0;
                        contShake++;
                        sinal = !sinal;
                    }
                }
                else
                {
                    estadoC = EstadoComplementarDaCamera.estabilizando;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoComplementarDaCamera.estabilizando:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido <= tempoDeShake)
                {
                    transform.localEulerAngles = EstabilizadorCondicional();
                }
                else
                {
                    estadoC = EstadoComplementarDaCamera.estavel;
                    transform.localEulerAngles = new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        0
                        );
                }
                break;
        }
    }

    Vector3 EstabilizadorCondicional()
    {
        Vector3 V = default;
        /*
        switch (axis)
        {
            default:
            case ShakeAxis.z:
                V = new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        Mathf.Lerp(transform.localEulerAngles.y, 0, tempoDecorrido / tempoDeShake)
                        );
            break;
            case ShakeAxis.x:
                V = new Vector3(
                    Mathf.Lerp(transform.localEulerAngles.y, 0, tempoDecorrido / tempoDeShake),
                        transform.localEulerAngles.y,
                        transform.localEulerAngles.z
                        );
            break;
            case ShakeAxis.y:
                V = new Vector3(
                    transform.localEulerAngles.x,
                    Mathf.Lerp(
                        transform.localEulerAngles.x, 0, tempoDecorrido / tempoDeShake),
                        transform.localEulerAngles.z
                        );
            break;
        }*/

        return V;
    }

    Vector3 ConditionalShake(Transform transform)
    {
        Vector3 V;
        switch (axis)
        {
            default:
            case ShakeAxis.z:
                V =new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        Mathf.Lerp(transform.localEulerAngles.y, (sinal ? 1 : -1) * shakeAngle, tempoDecorrido / tempoDeShake)
                        );
            break;
            case ShakeAxis.x:
                V = new Vector3(
                    Mathf.Lerp(transform.localEulerAngles.y, (sinal ? 1 : -1) * shakeAngle, tempoDecorrido / tempoDeShake),
                        transform.localEulerAngles.y,
                        transform.localEulerAngles.z
                        );
            break;
            case ShakeAxis.y:
                V = new Vector3(
                    transform.localEulerAngles.x,
                    Mathf.Lerp(transform.localEulerAngles.x, (sinal ? 1 : -1) * shakeAngle, tempoDecorrido / tempoDeShake),
                        transform.localEulerAngles.z
                        );
            break;

        }

        return V;
    }
}

public enum ShakeAxis
{
    x,y,z
}
