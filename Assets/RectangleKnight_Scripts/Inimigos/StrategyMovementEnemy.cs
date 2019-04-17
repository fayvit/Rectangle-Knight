using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrategyMovementEnemy : NonRespawnOnLoadEnemy
{

    private EstadoDaqui estado = EstadoDaqui.emEspera;
    private Rigidbody2D r2;
    private Vector3 moveDir;
    private Vector3 moveVel;
    private float tempoDecorrido = 0;

    [SerializeField] private float INTERVALO_DE_TIRO = 4;
    [SerializeField] private float DISTANCIA_ATIVACAO = 30;
    [SerializeField] private float TRANSICAO_DA_VELOCIDADE = 5;
    [SerializeField] private float TEMPO_TELEGRAFANDO = 0.5F;
    [SerializeField] private float VEL_MOVIMENTO = 2F;
    [SerializeField] private float TEMPO_NA_ESPERA = 1.5F;
    [SerializeField] private float TEMPO_EM_EVASIVA = 3F;
    [SerializeField] private float TEMPO_DE_VERIFICAR_SE_PERTO = 1F;


    [SerializeField] GameObject mark1;
    [SerializeField] GameObject mark2;

    private enum EstadoDaqui
    {
        emEspera,
        posicionandoParaAtirar,
        posicionandoEvasivamente,
        telegrafando


    }

    protected abstract void RequestAction(Vector3 charPos);

    protected abstract void Telegrafar(Vector3 charPos);

    protected override void Start()
    {

        Invoke("VerificaDistanciaDeAtivacao", TEMPO_DE_VERIFICAR_SE_PERTO);
        r2 = GetComponent<Rigidbody2D>();
        base.Start();
    }

    void VerificaDistanciaDeAtivacao()
    {
        if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < DISTANCIA_ATIVACAO)
        {
            VerifiquePosicionamento();
            estado = EstadoDaqui.posicionandoParaAtirar;
        }
        else
            Invoke("VerificaDistanciaDeAtivacao", TEMPO_DE_VERIFICAR_SE_PERTO);

    }

    void VerifiquePosicionamento()
    {
        int[] angulos = new int[9] { 0, 30, -30, 45, -45, 60, -60, 90, -90 };
        int cont = 0;
        bool foi = false;
        Vector3 charPosition = GameController.g.Manager.transform.position;
        Vector3 baseDir = charPosition - transform.position;
        Vector3 posDeInteresse = Vector3.zero;

        while (cont < 9 && !foi)
        {

            Vector3 dir = Quaternion.AngleAxis(angulos[cont], Vector3.forward) * baseDir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 100, 511);

            if (hit)
            {

                /*
                Debug.Log(cont+" raycast true "+hit.transform.name);
                Instantiate(mark1, hit.point, Quaternion.identity);
                */
                Vector3 thisPoint = hit.point;
                hit = Physics2D.Linecast(hit.point + 0.25f * hit.normal, charPosition, 511);

                if (!hit)
                {
                    // Instantiate(transform.GetChild(0), thisPoint, Quaternion.identity);
                    moveDir = (thisPoint - transform.position).normalized;

                    foi = true;
                }
                else
                {
                    moveDir = dir;

                    /*
                    Debug.Log(cont + "licast true: " + hit.transform.name);
                    Instantiate(mark2, hit.point, Quaternion.identity);*/

                }
            }
            else
            {
                hit = Physics2D.Linecast(transform.position, charPosition, 511);
                if (hit)
                {
                    //Instantiate(transform.GetChild(0), charPosition, Quaternion.identity);

                    moveDir = -dir;
                    foi = true;
                }
                else
                    foi = false;
            }

            cont++;
        }

        if (foi)
        {
            estado = EstadoDaqui.posicionandoParaAtirar;
        }
        else
        {
            moveDir = (transform.position - charPosition).normalized;
            estado = EstadoDaqui.posicionandoEvasivamente;
            Invoke("VerificaDistanciaDeAtivacao", TEMPO_EM_EVASIVA);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 charPos = default;
        if (GameController.g)
            if (GameController.g.Manager)
                charPos = GameController.g.Manager.transform.position;

        switch (estado)
        {
            case EstadoDaqui.emEspera:
                moveVel = Vector3.Lerp(moveVel, moveDir, TRANSICAO_DA_VELOCIDADE * Time.deltaTime);
                r2.velocity = VEL_MOVIMENTO * moveVel;

                FlipDirection.Flip(transform, moveVel.x);
            break;
            case EstadoDaqui.posicionandoParaAtirar:
                tempoDecorrido += Time.deltaTime;
                moveVel = Vector3.Lerp(moveVel, moveDir, TRANSICAO_DA_VELOCIDADE * Time.deltaTime);

                FlipDirection.Flip(transform, moveVel.x);

                r2.velocity = VEL_MOVIMENTO * moveVel;

                if (tempoDecorrido > INTERVALO_DE_TIRO)
                {
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, GameController.g.Manager.transform.position, 511);

                    if (!hit)
                    {
                        tempoDecorrido = 0;
                        estado = EstadoDaqui.telegrafando;

                        Telegrafar(charPos);
                    }
                    else
                    {
                        moveDir = (transform.position - charPos).normalized;
                        estado = EstadoDaqui.emEspera;
                        tempoDecorrido = 0;
                        Invoke("VerifiquePosicionamento", TEMPO_NA_ESPERA);
                    }
                }
                break;
            case EstadoDaqui.telegrafando:
                tempoDecorrido += Time.deltaTime;
                moveVel = Vector3.Lerp(moveVel, Vector3.zero, TRANSICAO_DA_VELOCIDADE * Time.deltaTime);
                r2.velocity = moveVel;

                //FlipDirection.Flip(transform, moveVel.x);

                if (tempoDecorrido > TEMPO_TELEGRAFANDO)
                {
                    RequestAction(charPos);
                }
            break;
            case EstadoDaqui.posicionandoEvasivamente:
                moveVel = Vector3.Lerp(moveVel, moveDir, TRANSICAO_DA_VELOCIDADE * Time.deltaTime);
                r2.velocity = VEL_MOVIMENTO * moveVel;

                FlipDirection.Flip(transform, moveVel.x);

            break;
        }
    }

    

    protected void RetornarParaEsperaZerandoTempo()
    {
        estado = EstadoDaqui.emEspera;
        tempoDecorrido = 0;
        VerifiquePosicionamento();
    }
}
