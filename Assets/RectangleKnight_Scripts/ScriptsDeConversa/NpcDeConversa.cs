using UnityEngine;
using System.Collections;

[System.Serializable]
public class NPCdeConversa
{
    [SerializeField] private Transform[] pontosAlvo;
    [SerializeField] private ChaveDeTexto chaveDaConversa = ChaveDeTexto.bomDia;

    [SerializeField] protected Sprite fotoDoNPC;
    [SerializeField] private int modificarIndiceDeInicio = 0;

    protected EstadoDoNPC estado = EstadoDoNPC.parado;
    protected string[] conversa;

    private Transform meuTransform;
    
    //private SigaOLider siga;

    private Vector3 alvo = Vector3.zero;
    private float tempoParado = 0.5f;
    private float contadorDeTempo = 0;

    private const float TEMPO_MAX_PARADO = 20;
    private const float TEMPO_MIN_PARADO = 8;
    protected enum EstadoDoNPC
    {
        caminhando,
        parado,
        conversando,
        finalizadoComBotao
    }

    public void MudaChaveDaConversa(ChaveDeTexto chave)
    {
        conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(chave).ToArray();
    }

    void OnEnable()
    {
        if (pontosAlvo != null)
            if (pontosAlvo.Length > 0)
                alvo = pontosAlvo[Random.Range(0, pontosAlvo.Length)].position;
    }

    public void Start(Transform T)
    {

        meuTransform = T;
        conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(chaveDaConversa).ToArray();
        /*

        if (siga == null)
        {
            siga = new SigaOLider(T, 0.75f, 2, 0.01f);
        }*/

        if (pontosAlvo == null)
            pontosAlvo = new Transform[1] { T };
        else if(pontosAlvo.Length==0)
            pontosAlvo = new Transform[1] { T };
    }

    // Update is called once per frame
    public virtual bool Update()
    {
        switch (estado)
        {
            case EstadoDoNPC.parado:
                contadorDeTempo += Time.deltaTime;
                if (contadorDeTempo > tempoParado)
                {
                    alvo = pontosAlvo[Random.Range(0, pontosAlvo.Length)].position;
                    contadorDeTempo = 0;
                    estado = EstadoDoNPC.caminhando;
                }
                break;
            case EstadoDoNPC.caminhando:
             //   siga.Update(alvo);
                if (Vector3.Distance(alvo, meuTransform.position) < 2)
                {
                   // siga.PareAgora();
                    estado = EstadoDoNPC.parado;
                    tempoParado = Random.Range(TEMPO_MIN_PARADO, TEMPO_MAX_PARADO);
                }
                break;
            case EstadoDoNPC.conversando:
                //AplicadorDeCamera.cam.FocarPonto(5);
                if (GameController.g.DisparaT.UpdateDeTextos(conversa/*, fotoDoNPC*/))
                {
                    FinalizaConversa();
                }
            break;
            case EstadoDoNPC.finalizadoComBotao:
                estado = EstadoDoNPC.parado;
            return true;
        }

        return false;
    }

    protected virtual void FinalizaConversa()
    {
        estado = EstadoDoNPC.finalizadoComBotao;
        //meuTransform.rotation = Quaternion.LookRotation(-Vector3.forward);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.finalizaDisparaTexto));

        /*
        GameController.g.HudM.ligarControladores();
        GameController.g.HudM.Botaozao.FinalizarBotao();
        GameController.g.HudM.DisparaT.DesligarPaineis();


        AndroidController.a.LigarControlador();
        */


    }

    public virtual void IniciaConversa()
    {
        // siga.PareAgora();

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.inicializaDisparaTexto));
        GameController.g.DisparaT.IniciarDisparadorDeTextos();
        GameController.g.DisparaT.IndiceDaConversa = modificarIndiceDeInicio;   

        /*
        GameController.g.HudM.DisparaT.IniciarDisparadorDeTextos();
        GameController.g.HudM.DisparaT.IndiceDaConversa = modificarIndiceDeInicio;
        GameController.g.HudM.Botaozao.IniciarBotao(FinalizaConversa,
            BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.ObrigadoComPressa)
            );*/
        estado = EstadoDoNPC.conversando;
    }
}
