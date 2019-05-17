using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XShooterEnemy : EnemyBase
{
    [SerializeField] private float vel = 5;
    [SerializeField] private float tempoTelegrafando = .35f;
    [SerializeField] private float tempoPosTiro = 0.5f;
    [SerializeField] private float intervaloDeTiro = 3;
    [SerializeField] private float distProjetilTransform = 1;
    [SerializeField] private float disAtivacao = 10;
    [SerializeField] private GameObject particulaTelegrafista = default;
    [SerializeField] private GameObject projetil = default;
    

    private Vector3 moveDirection;
    private Vector3 guardadorDePosicao;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        padrao,
        preparandoTiro,
        disparaTiro
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        guardadorDePosicao = transform.position;
        Invoke("VerifiqueAtivacao", 1);
        

        moveDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);

        if (moveDirection != Vector3.zero)
            moveDirection.Normalize();
        else
            moveDirection = new Vector3(1, 0, 0);

        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.Start();
    }

    void VerifiqueAtivacao()
    {
        if (Vector3.Distance(transform.position, GameController.g.Manager.transform.position) < disAtivacao)
        {
            estado = EstadoDaqui.padrao;
            Invoke("VerifiqueConstancia", 1);
            Invoke("TelegrafaTiro", 1.75f);
        }
        else
        {
            Invoke("VerifiqueAtivacao", 1);
        }
    }

    void TelegrafaTiro()
    {
        if (gameObject != null)
        {
            Invoke("TelegrafaTiro", intervaloDeTiro);

            estado = EstadoDaqui.preparandoTiro;
            InstanciaLigando.Instantiate(particulaTelegrafista, transform.position,5);
            EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, SoundEffectID.Wind1));

            new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, () =>
            {
                estado = EstadoDaqui.disparaTiro;
                Vector3[] dirs = new Vector3[4] {
                    new Vector3(1,1,0),
                    new Vector3(-1,1,0),
                    new Vector3(1,-1,0),
                    new Vector3(-1,-1,0)
                };
                for (int i = 0; i < 4; i++)
                {
                    GameObject G = InstanciaLigando.Instantiate(projetil, transform.position + distProjetilTransform *dirs[i], 5,
                        Quaternion.LookRotation(-projetil.transform.forward,Vector3.Cross(dirs[i], -Vector3.forward))
                        );

                    ProjetilInimigo P = G.AddComponent<ProjetilInimigo>();
                    P.Iniciar(dirs[i], particulaTelegrafista, 10f);
                    P.SomDeImpacto = SoundEffectID.lancaProjetilInimigo;

                    new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject, () => {
                        estado = EstadoDaqui.padrao;
                    }, tempoPosTiro);
                }
            }, tempoTelegrafando);
        }
    }

    void VerifiqueConstancia()
    {
        if (gameObject != null)
        {
            if (Vector3.Distance(transform.position, guardadorDePosicao) < 0.25f)
            {
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
        hit[0] = Physics2D.Raycast(transform.position, new Vector2(1, 1), 100, 511);
        hit[1] = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 100, 511);
        hit[2] = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 100, 511);
        hit[3] = Physics2D.Raycast(transform.position, new Vector2(1, -1), 100, 511);

        retorno = hit[1].point;

        for (int i = 1; i < 4; i++)
        {
            if (Vector3.Distance(transform.position, retorno) < Vector3.Distance(transform.position, hit[i].point))
            {
                retorno = hit[i].point;
            }
        }
        return retorno - transform.position;
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
        base.OnDestroy();
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        if (e.Sender.transform.IsChildOf(transform))
        {

        }
    }

    /*
    protected override void OnReceivedDamageAmount(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        //Debug.Log(e.Sender.transform.IsChildOf(transform) + " : " + transform.IsChildOf(e.Sender.transform));

        if (gameObject.layer == 11 && e.Sender.transform.IsChildOf(transform))
        {
            AplicaDano((int)ssge.MyObject[0]);
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.padrao:

                transform.position += vel * moveDirection * Time.deltaTime;

                Debug.DrawRay(nPoint, preDir, Color.blue);
                Debug.DrawRay(nPoint, -antDir, Color.red);
                Debug.DrawRay(nPoint, nDir, Color.green);
            break;
        }

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
            moveDirection = Quaternion.AngleAxis(multiply * variation, Vector3.forward) * moveDirection;
            moveDirection.Normalize();
            //Debug.Log(moveDirection);
        }
    }

    /*
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
    }*/
}

