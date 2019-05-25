using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoZigZag : EnemyBase
{
    [SerializeField] private float vel = 5;
    private Vector3 moveDirection;
    private Vector3 guardadorDePosicao;
    

    // Start is called before the first frame update
     protected override void Start()
    {

        guardadorDePosicao = transform.position;
        if (gameObject.layer == 11)
        {
            moveDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);

            if (moveDirection != Vector3.zero)
                moveDirection.Normalize();
            else
                moveDirection = new Vector3(1, 0, 0);
        }

        Invoke("VerifiqueConstancia",1);
        base.Start();
    }

    void VerifiqueConstancia()
    {
        if (gameObject != null)
        {
            if (Vector3.Distance(transform.position, guardadorDePosicao) < 0.25f)
            {
                Debug.Log("constancia identificada");
                moveDirection = BuscadorDeDirecao().normalized;
            }

            guardadorDePosicao = transform.position;
            Invoke("VerifiqueConstancia", 1);
        }
    }

    Vector3 BuscadorDeDirecao()
    {
        Vector3 retorno = default;
        RaycastHit2D[] hit = new RaycastHit2D[4];
        hit[0] = Physics2D.Raycast(transform.position, new Vector2(1, 1),100,511);
        hit[1] = Physics2D.Raycast(transform.position, new Vector2(-1, 1),100,511);
        hit[2] = Physics2D.Raycast(transform.position, new Vector2(-1, -1),100,511);
        hit[3] = Physics2D.Raycast(transform.position, new Vector2(1, -1),100,511);

        retorno = hit[1].point;

        for (int i = 1; i < 4; i++)
        {
            Debug.Log("colidiu com: "+hit[i].collider.name);
            if (Vector3.Distance(transform.position, retorno) < Vector3.Distance(transform.position, hit[i].point))
            {
                retorno = hit[i].point;
            }
        }
        return retorno-transform.position;
    }

    protected override void OnReceivedDamageAmount(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        //Debug.Log(e.Sender.transform.IsChildOf(transform) + " : " + transform.IsChildOf(e.Sender.transform));

        if (gameObject.layer == 11 && e.Sender.transform.IsChildOf(transform))
        {
            AplicaDano((int)ssge.MyObject[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == 11)
            transform.position += vel * moveDirection *Time.deltaTime;

        Debug.DrawRay(nPoint, preDir,Color.blue);
        Debug.DrawRay(nPoint, -antDir,Color.red);
        Debug.DrawRay(nPoint, nDir, Color.green);

    }

    Vector3 nPoint = default;
    Vector3 nDir = default;
    Vector3 antDir = default;
    Vector3 preDir = default;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            Vector3 normal3 = collision.contacts[0].normal;
            Vector3 proj = Vector3.Project(moveDirection, normal3);
            Vector3 myDIr = -(2 * proj - moveDirection);

            nPoint = collision.contacts[0].point;
            nDir = collision.contacts[0].normal;
            antDir = moveDirection;
            preDir = myDIr;

            
            float variation = Random.Range(30f, 45f);
            int multiply = Random.Range(0, 2) == 0 ? 1 : -1;
            moveDirection = myDIr;//-2 * moveDirection - new Vector3(collision.contacts[0].normal.x, collision.contacts[0].normal.y,0);
            moveDirection = Quaternion.AngleAxis(multiply*variation,Vector3.forward)* moveDirection;
            moveDirection.Normalize();
            //Debug.Log(moveDirection);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision.name + " : " + collision.tag);
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage, sentidoPositivo, Dados.AtaqueBasico));
            }
        }


        if (collision.tag == "attackCollisor" && gameObject.layer==11)
        {
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage, collision.name));
        }
    }
}
