using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaProjetil : MonoBehaviour
{
    [SerializeField] private float velocidade = 20;

    private Vector3 dir;

    public GameObject Particle { get; set; }

    public void Iniciar(float dir, GameObject particle,float velocidade = 20)
    {
        
        Iniciar(dir * Vector3.right, particle,20);
    }

    public void Iniciar(Vector3 dir, GameObject particle,float velocidade)
    {
        this.velocidade = velocidade;
        this.dir = dir;
        this.dir.Normalize();
        this.Particle = particle;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir  * velocidade * Time.deltaTime;
    }
}
