using UnityEngine;
using System.Collections;

public abstract class AtivadorDeBotao : MonoBehaviour
{
    [SerializeField] protected GameObject btn;
    [SerializeField] protected float distanciaParaAcionar = 4.6f;
    protected string textoDoBotao = "";
    private bool estaNoTrigger = false;

    public GameObject Btn { get => btn; }

    // Use this for initialization
    void Start()
    {

    }

    protected void FluxoDeBotao()
    {
       // GameController.EntrarNoFluxoDeTexto();

        /*
        GameController.g.Manager.transform.rotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(transform.position - GameController.g.Manager.transform.position, Vector3.up));
            */

        Update();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (name == "Hexagonos")
            //Debug.Log(Vector3.Distance(GameController.g.Manager.transform.position, transform.position) +" : "+ distanciaParaAcionar+" : "+estaNoTrigger+" : "+ ActionManager.PodeVisualizarEste(this));
        if (GameController.g)
            if (GameController.g.Manager)
                if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) < distanciaParaAcionar
                    &&
                    estaNoTrigger
                    &&
                    GameController.g.Manager.Estado == EstadoDePersonagem.aPasseio
                    &&
                    ActionManager.PodeVisualizarEste(this)
                  //  &&
                   // GameController.g.EmEstadoDeAcao()
                    &&
                    gameObject.activeSelf
                    )
                {

                    btn.SetActive(true);
                }
                else
                {
                    /*
                    if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) >= distanciaParaAcionar)
                    {*/
                        btn.SetActive(false);
                  /*  }else
                    if (ActionManager.TransformDeActionE(transform))
                    {

                        btn.SetActive(true);
                    }*/
                        
                }

    }
    protected void SempreEstaNoTrigger()
    {
        estaNoTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            estaNoTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            estaNoTrigger = false;
        }
    }

    /*
    public virtual void SomDoIniciar()
    {
        EventAgregator.Publish(new StandardSendStringEvent(gameObject, SoundEffectID.Decision1.ToString(), EventKey.disparaSom));
    }*/

    public abstract void FuncaoDoBotao();
}
