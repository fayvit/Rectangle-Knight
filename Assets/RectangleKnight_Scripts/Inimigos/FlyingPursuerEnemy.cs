using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPursuerEnemy : EnemyBase
{
    private bool perseguir = false;
    private bool ligado = false;
    private Rigidbody2D r2;
    private Vector3 dirDeMovimento;
    private Vector3 redirecionador = default;

    [SerializeField] private float DISTANCIA_PERSEGUICAO = 30;
    [SerializeField] private float DISTANCIA_ATIVACAO = 17;
    [SerializeField] private float VARIACAO_DA_DIRECAO = 0.5F;
    [SerializeField] private float VEL_DO_MOVIMENTO = 4;
    [SerializeField] private float multiplicadorDoRedirecionamento = 50;

    protected override void Start()
    {
        r2 = GetComponent<Rigidbody2D>();
        Invoke("DistanciaDoJogador", 2);
        base.Start();
    }

    void DistanciaDoJogador()
    {
        if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < DISTANCIA_ATIVACAO)
            ligado = true;

        if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < DISTANCIA_PERSEGUICAO)
            perseguir = true;
        else
            perseguir = false;

        redirecionador = Vector3.zero;
        Invoke("DistanciaDoJogador", 2);
    }

    protected override void OnReceivedDamageAmount(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

       // Debug.Log(e.Sender.transform.IsChildOf(transform) + " : " + transform.IsChildOf(e.Sender.transform));

        if (gameObject.layer == 11 && e.Sender.transform.IsChildOf(transform))
        {
            AplicaDano((int)ssge.MyObject[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (r2)
        {
            if (perseguir && ligado)
                dirDeMovimento = VEL_DO_MOVIMENTO * (
                    Vector3.Lerp(dirDeMovimento, GameController.g.Manager.transform.position - transform.position + redirecionador,
                    VARIACAO_DA_DIRECAO * Time.deltaTime).normalized);
            else
                dirDeMovimento = Vector3.Lerp(dirDeMovimento, Vector3.zero, VARIACAO_DA_DIRECAO * Time.deltaTime);

            r2.velocity = dirDeMovimento;

            Vector3 V = transform.localScale;
            if (r2.velocity.x > 0)
            {

                transform.localScale = new Vector3(-1 * Mathf.Abs(V.x), V.y, V.z);
            }
            else
                transform.localScale = new Vector3(Mathf.Abs(V.x), V.y, V.z);
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


        if (collision.tag == "attackCollisor" && gameObject.layer == 11)
        {
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage, collision.name));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            
            redirecionador = multiplicadorDoRedirecionamento*collision.contacts[0].normal;
        }
    }
}
