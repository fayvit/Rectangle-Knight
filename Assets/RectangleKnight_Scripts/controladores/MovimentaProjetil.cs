using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaProjetil : MonoBehaviour
{
    [SerializeField] private float velocidade = 20;
    [SerializeField]private Vector3 dir = default;

    public GameObject Particle { get; set; }
    public float Velocidade { get => velocidade; set => velocidade = value; }
    public Vector3 Dir { get => dir; set => dir = value; }

    public void Iniciar(float dir, GameObject particle,float velocidade = 20)
    {
        
        Iniciar(dir * Vector3.right, particle,velocidade);
    }

    public void Iniciar(Vector3 dir, GameObject particle,float velocidade)
    {
        this.Velocidade = velocidade;
        this.Dir = (new Vector3(dir.x, dir.y, 0)).normalized;
        this.Particle = particle;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += Dir  * Velocidade * Time.deltaTime;
    }
}
