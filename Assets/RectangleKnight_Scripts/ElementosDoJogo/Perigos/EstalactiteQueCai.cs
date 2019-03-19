using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstalactiteQueCai : MonoBehaviour
{
    private bool iniciou = false;
#pragma warning disable 0649
    [SerializeField]private Rigidbody2D rg2d;
#pragma warning restore 0649
    // Update is called once per frame
    void Update()
    {
        if (iniciou)
        {
            rg2d.gravityScale = 2;
            rg2d.simulated = true;
        }
    }

    public void Iniciar()
    {
        iniciou = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            CapsuleCollider2D cc2d = null;

            try { cc2d = (CapsuleCollider2D)collision; }
            catch { }

            if (cc2d != null)
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, 15));
            }
        }
        else if(collision.name=="finalizador")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
