using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaMagia : MonoBehaviour
{
    [SerializeField] private float velocidade = 20;

    private float dir;
    private GameObject particle;

    public void Iniciar(float dir,GameObject particle)
    {
        this.dir = dir;
        this.particle = particle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir*Vector3.right * velocidade * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" && collision.tag != "Player")
        {

            GameObject g = Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(g, 2);
            g.SetActive(true);
                
            Destroy(gameObject);
        }
    }
}
