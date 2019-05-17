using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingInvestEnemy : EnemyBase
{
    private float ultimaInvestida = 0;
    private Vector3 dirDeMovimento;
    private Vector3 dirGuardada;
    private Vector3 redirecionador = default;
    private Rigidbody2D r2 = default;
    private Quaternion qAlvo;
    private EstadoDaqui estado =EstadoDaqui.emEspera;

    [SerializeField] private AudioClip impulso = default;
    [SerializeField] private float DISTANCIA_PERSEGUICAO = 30;
    //[SerializeField] private float DISTANCIA_ATIVACAO = 17;
    [SerializeField] private float VARIACAO_DA_DIRECAO = 0.5F;
    [SerializeField] private float AMORTECIMENTO_PARA_INVESTIDA = 5F;
    [SerializeField] private float VEL_DO_MOVIMENTO = 4;
    [SerializeField] private float VEL_DA_INVESTIDA= 7;
    [SerializeField] private float DISTANCIA_DO_INVESTIMENTO = 10;
    [SerializeField] private float INTERVALO_DE_INVESTIMENTOS = 3;
    [SerializeField] private float TEMPO_PREPARANDO = 0.2F;
    [SerializeField] private float TEMPO_COLIDINDO = .5F;
    [SerializeField] private float multiplicadorDoRedirecionamento = 50;

    private enum EstadoDaqui
    {
        emEspera,
        perseguindo,
        preparandoInvestida,
        investindo,
        animandoColisao
    }
    protected override void Start()
    {
        r2 = GetComponent<Rigidbody2D>();
        Invoke("DistanciaDoJogador", 2);
        base.Start();
    }

    void DistanciaDoJogador()
    {
        if (estado == EstadoDaqui.emEspera || estado == EstadoDaqui.perseguindo)
        {
            EstadoDaqui era = estado;
            if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < DISTANCIA_PERSEGUICAO)
                estado = EstadoDaqui.perseguindo;
            else
                estado = EstadoDaqui.emEspera;

            if (era == EstadoDaqui.emEspera && estado == EstadoDaqui.perseguindo)
                ultimaInvestida = Time.time;

            redirecionador = Vector3.zero;

            VerifiqueInvestida();
        }

        
        Invoke("DistanciaDoJogador", 2);
    }

    void VerifiqueInvestida()
    {
        
        if (Time.time - ultimaInvestida > INTERVALO_DE_INVESTIMENTOS &&
            Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < DISTANCIA_DO_INVESTIMENTO)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, GameController.g.Manager.transform.position, 511);
            if (!hit)
            {
                ultimaInvestida = Time.time;
                _Animator.SetTrigger("preparaInvestida");
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Wind1));
                estado = EstadoDaqui.preparandoInvestida;
                dirGuardada = GameController.g.Manager.transform.position - transform.position;
                dirGuardada.Normalize();
                
                qAlvo = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(dirGuardada, Vector3.forward));
                FlipDirection.Flip(transform, dirDeMovimento.x);
                new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,() =>
                {
                    
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, impulso));
                    dirGuardada = GameController.g.Manager.transform.position - transform.position;
                    dirGuardada.Normalize();
                    dirGuardada *= VEL_DA_INVESTIDA;

                    transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(dirGuardada, Vector3.forward));
                    estado = EstadoDaqui.investindo;
                    
                }, TEMPO_PREPARANDO);
            }
        }
    }

    
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.emEspera:
                dirDeMovimento = Vector3.Lerp(dirDeMovimento, Vector3.zero, VARIACAO_DA_DIRECAO * Time.deltaTime);
                ApliqueMovimento();
            break;
            case EstadoDaqui.perseguindo:
                dirDeMovimento = VEL_DO_MOVIMENTO * (
                    Vector3.Lerp(dirDeMovimento, GameController.g.Manager.transform.position - transform.position + redirecionador,
                    VARIACAO_DA_DIRECAO * Time.deltaTime).normalized);
                ApliqueMovimento();
            break;
            case EstadoDaqui.preparandoInvestida:

                dirDeMovimento = Vector3.Lerp(dirDeMovimento, Vector3.zero, AMORTECIMENTO_PARA_INVESTIDA * Time.deltaTime);
                ApliqueMovimento();

                transform.rotation = Quaternion.Lerp(transform.rotation, qAlvo, 3*Time.deltaTime);
            break;
            case EstadoDaqui.investindo:
                dirDeMovimento = dirGuardada;
                ApliqueMovimento();
            break;
        }
    }

    void ApliqueMovimento()
    {
        r2.velocity = dirDeMovimento;

        Vector3 V = transform.localScale;
        if (r2.velocity.x > 0)
        {

            transform.localScale = new Vector3(-1 * Mathf.Abs(V.x), V.y, V.z);
        }
        else
            transform.localScale = new Vector3(Mathf.Abs(V.x), V.y, V.z);
    }

    /*
    protected void OnReceivedTriggerInfo(IGameEvent e)
    {
        if (e.Sender.transform.IsChildOf(transform))
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;
            Collider2D collision = (Collider2D)ssge.MyObject[0];
            //OnTriggerEnter2D(collision);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {

            redirecionador = multiplicadorDoRedirecionamento * collision.contacts[0].normal;

            if (estado == EstadoDaqui.investindo)
            {
                estado = EstadoDaqui.animandoColisao;
                _Animator.SetTrigger("colidiu");
                EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.request3dSound, SoundEffectID.Break));
                new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,() =>
                {
                    estado = EstadoDaqui.perseguindo;
                    transform.rotation = Quaternion.identity;
                },TEMPO_COLIDINDO);
            }
        }
    }
}
