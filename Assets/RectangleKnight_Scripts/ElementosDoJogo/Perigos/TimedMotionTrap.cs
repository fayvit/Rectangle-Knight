using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedMotionTrap : MonoBehaviour
{
    #region inpector
    [SerializeField] private float loopTime = 2;
    [SerializeField] private float standTime = 0.25f;
    [SerializeField] private float startDelay = 0;
    [SerializeField] private Transform[] pos = default;
    [SerializeField] private AudioSource doSomUm = default;
    [SerializeField] private AudioSource doSomDois = default;
    #endregion

    private EstadoDaqui estado = EstadoDaqui.movimentando;
    private Vector3 posAuxGuardada;
    private float tempoDecorrido = 0;
    private int indiceDaPos = 0;
    private bool somUm;
    private bool somDois;

    private enum EstadoDaqui
    {
        movimentando,
        esperando
    }
    // Start is called before the first frame update
    void Start()
    {
        posAuxGuardada = transform.position;
        tempoDecorrido = -startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        tempoDecorrido += Time.deltaTime;

        switch (estado)
        {
            case EstadoDaqui.movimentando:
                if (tempoDecorrido < loopTime)
                {
                    float interpolation = //ZeroOneInterpolation.PolinomialInterpolation(tempoDecorrido / loopTime, 12);
                        
                        //ZeroOneInterpolation.RadicalOddInterpolation(tempoDecorrido / loopTime,3);
                        
                        ZeroOneInterpolation.LagrangeInterppolation(tempoDecorrido / loopTime,
                        new Vector2(0.75f, 1),
                        new Vector2(0.95f,0.65f));

                    if (tempoDecorrido > 0f && !somUm)
                    {
                        doSomUm.Play();
                        somUm = true;
                    }

                    if (tempoDecorrido > 1.75f && !somDois)
                    {
                        doSomDois.Play();
                        somDois = true;
                    }

                    //Debug.Log(interpolation + " : " + (tempoDecorrido / loopTime));

                    transform.position = Vector3.Lerp(posAuxGuardada, pos[indiceDaPos].position,
                        interpolation
                        );

                    
                }
                else
                {
                    estado = EstadoDaqui.esperando;
                    tempoDecorrido = 0;
                    somUm = false;
                    somDois = false;
                }
            break;
            case EstadoDaqui.esperando:
                if (tempoDecorrido > standTime)
                {
                    tempoDecorrido = 0;
                    indiceDaPos = ContadorCiclico.AlteraContador(1, indiceDaPos, pos.Length);
                    posAuxGuardada = transform.position;
                    estado = EstadoDaqui.movimentando;

                }
            break;
        }
    }
}
