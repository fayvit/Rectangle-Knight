using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaMagia : MovimentaProjetil
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" && collision.tag != "Player" && collision.tag!="triggerGeral"&&collision.gameObject.layer!=11)
        {
            Debug.Log(collision.name + " : " + collision.tag + " : " + collision.gameObject.layer);

            GameObject g = Instantiate(Particle, transform.position, Quaternion.identity);
            Destroy(g, 2);
            g.SetActive(true);
                
            Destroy(gameObject);
        }
    }
}
