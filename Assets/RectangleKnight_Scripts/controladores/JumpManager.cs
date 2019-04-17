using UnityEngine;

[System.Serializable]
public class JumpManager
{
    [SerializeField] private float tempoMaxPulo = 1;
    [SerializeField] private float alturaDoPulo = 3;
    [SerializeField] private float velocidadeSubindo = 9;

#pragma warning disable 0649
    [SerializeField] private ParticleSystem doublejumpParticles;
    [SerializeField] private AudioClip somDoPuloDuplo;
#pragma warning restore 0649

    private float tempoDePulo = 0;
    private bool EstavaPulando;
    private Rigidbody2D rg2d;

    public bool EstouPulando { get; private set; }

    public bool EstouSubindo { get; private set; }

    public bool PodePuloDuplo { get; private set; }

    public float UltimoYFundamentado { get; private set; }
    
    public bool AlcancouTempoMin
    {
        get
        {
            if (!EstouPulando)
                return true;
            else if (tempoDePulo < 0.25f* tempoMaxPulo)
                return false;
            else return true;
        }
    }

    public void IniciarCampos(Rigidbody2D r2)
    {
        rg2d = r2;
    }

    public void IniciaAplicaPulo(float ultimoYFundamentado)
    {
        UltimoYFundamentado = ultimoYFundamentado;
        EstouPulando = true;
        EstouSubindo = true;
        PodePuloDuplo = true;
        //Move(Vector3.up * impulsoInicial);
        
    }

    public void IniciaAplicaPuloDuplo(float ultimoYFundamentado)
    {
        UltimoYFundamentado = ultimoYFundamentado;
        EstouPulando = true;
        EstouSubindo = true;
        PodePuloDuplo = false;
        doublejumpParticles.gameObject.SetActive(true);
        doublejumpParticles.Play();

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoPuloDuplo));
    }

    public void VerificaPulo()
    {

        if (EstavaPulando == false && EstouPulando == true)
        {
            tempoDePulo = 0;
            EstouSubindo = true;
        }

        EstavaPulando = EstouPulando;
        tempoDePulo += Time.deltaTime;

        /*
        if (elementos.Controle.gameObject.name == "esperandoTeste")
            Debug.Log(
                EstouSubindo
                + " : " +
                elementos.transform.position.y + " : " + UltimoYFundamentado + " : " + caracteristicas.alturaDoPulo
             + " : " +
             tempoDePulo + " : " + caracteristicas.tempoMaxPulo
             );
*/

        if (
            EstouSubindo == true
            &&
            rg2d.transform.position.y - UltimoYFundamentado < alturaDoPulo
         &&
         tempoDePulo < tempoMaxPulo
         )
        {

            Move(velocidadeSubindo);

        }
        else if (
          (rg2d.transform.position.y - UltimoYFundamentado >= alturaDoPulo
       ||
       tempoDePulo >= tempoMaxPulo
       )
          &&
          EstouSubindo == true)
        {
            EstouSubindo = false;
            Move(velocidadeSubindo);
        }
        else if (EstouSubindo == false)
        {
            /*
            velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, velMax, amortecimento * Time.deltaTime);
            elementos.Controle.Move((direcaoMovimento * targetSpeed + velocidadeDescendo * Vector3.down) * Time.deltaTime);
            */
            /*
            movimentoVertical = Vector3.Lerp(movimentoVertical,
                                             (
                                              Vector3.down * velocidadeDescendo),
                                             amortecimentoNaTransicaoDePulo );
            Move((direcaoMovimento * velocidadeDuranteOPulo + movimentoVertical));
            */
        }

        /*
        if (elementos.Controle.gameObject.name == "esperandoTeste")
            Debug.Log(elementos.Controle.collisionFlags);

        
        if (elementos.Controle.collisionFlags == CollisionFlags.CollidedAbove)
            EstouSubindo = false;*/

    }

    public void NaoEstouPulando()
    {
        /*
        if (caracteristicas.estouPulando)
            elementos.Animador.SetBool("pulo", false);
*/

        EstouPulando = false;
        EstavaPulando = false;
    }

    void Move(float V)
    {
        rg2d.velocity = new Vector2(rg2d.velocity.x, V);
    }

    
}