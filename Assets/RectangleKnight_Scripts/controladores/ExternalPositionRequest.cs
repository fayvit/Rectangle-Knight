using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalPositionRequest
{
    private Transform dono;
    private MovimentacaoBasica mov;
    private Vector3 target;
    private float vel = 1;

    public ExternalPositionRequest(Transform dono, MovimentacaoBasica mov)
    {
        this.dono = dono;
        this.mov = mov;
    }

    public GameObject Requisitor { get; private set; }

    public void RequererMovimento(GameObject requisitor,Vector3 target,float vel)
    {
        Requisitor = requisitor;
        this.target = target;
        this.vel = vel;
    }

    public bool UpdateMove()
    {
        Vector3 dir = DirecaoNoPlano.NoUpNormalizado(dono.position, target);
        float distancia = Mathf.Abs(dono.position.x-target.x);
        float newVel = distancia>0.5f?vel:vel*distancia;

        mov.AplicadorDeMovimentos(newVel * dir);

        if (distancia < 0.1f)
            return true;

        return false;
    }
}
