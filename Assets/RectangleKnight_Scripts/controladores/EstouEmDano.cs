using UnityEngine;
using System.Collections;

[System.Serializable]
public class EstouEmDano
{
    private float tempoDeDano = 0;
    
    private float alturaAtual;
    
    private Vector3 posInicial;
    private Vector3 direcao = Vector3.zero;

    private Rigidbody2D controle;

    private const float alturaAlvo = 1;
    private const float tempoBase = 0.15f;
    private const float distanciaAlvo = 2;
    private const float m_MaxSpeed = 15f;
    private const float tempoFinal = 0.25f;
    
    
    /*
   
    private float alturaDoDano;
    
    private Vector3 vMove = Vector3.zero;
      
    */



    // Use this for initialization
    public EstouEmDano(Rigidbody2D controle)
    {
        this.controle = controle;
        //animator = controle.GetComponent<Animator>();
    }

    public void Start(Vector3 posInicial,Vector3 direcao)//, float alturaDoDano)//, IGolpeBase golpe)
    {
      //  esseGolpe = golpe;
        tempoDeDano = 0;
        this.posInicial = posInicial;
        this.direcao = direcao;
        //this.alturaDoDano = alturaDoDano;

    }



    // Update is called once per frame
    public bool Update(MovimentacaoBasica mov,Vector3 vetorDirecao)
    {
        Transform transform = controle.transform;

        tempoDeDano += Time.deltaTime;

        alturaAtual = transform.position.y;
        //direcao = Vector3.zero;

        if ((alturaAtual < posInicial.y + alturaAlvo)
            && Mathf.Abs(transform.position.x-posInicial.x)<distanciaAlvo
            )
        {
            if (tempoDeDano < tempoBase)
            {
                controle.velocity = new Vector2(direcao.x, direcao.y) * m_MaxSpeed;
            }
            else
            {
                vetorDirecao = vetorDirecao + direcao.normalized;
                mov.AplicadorDeMovimentos(vetorDirecao.normalized, false,false);
            }            
        }

        if (tempoDeDano < tempoFinal)
        {
            return false;
        }
        /*
        if (alturaAtual < alturaDoDano + 0.5f)
        {
            direcao += 12 * Vector3.up;
        }
        if ((transform.position - posInicial).sqrMagnitude < esseGolpe.DistanciaDeRepulsao)
            direcao += esseGolpe.VelocidadeDeRepulsao * esseGolpe.DirDeREpulsao;

        vMove = Vector3.Lerp(vMove, direcao, 10 * Time.deltaTime);
        controle.Move(vMove * Time.deltaTime);

        
        if (tempoDeDano > esseGolpe.TempoNoDano)
        {
            return false;
            
        }*/

        return true;
    }


}
