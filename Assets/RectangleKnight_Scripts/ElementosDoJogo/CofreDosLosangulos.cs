using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreDosLosangulos : MonoBehaviour
{
    #region inspector
    [SerializeField, Range(0, 100)] private int inicioDeAcao = default;
    [SerializeField, Range(0, 100)] private int finalDeAcao = default;
    [SerializeField] private GameObject premio = default;
    [SerializeField] private Sprite spriteAberto = default;
    #endregion

    public int InicioDeAcao { get => inicioDeAcao; private set => inicioDeAcao = value; }
    public int FinalDeAcao { get => finalDeAcao; private set => finalDeAcao = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.g.MyKeys.VerificaCont(KeyCont.losangulosConfirmados) >= FinalDeAcao)
            GetComponent<SpriteRenderer>().sprite = spriteAberto;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KeyVar myKeys = GameController.g.MyKeys;
        if (collision.tag == "Player")
        {
            if(myKeys.VerificaCont(KeyCont.losangulosPegos) >= InicioDeAcao
                &&
                myKeys.VerificaCont(KeyCont.losangulosConfirmados)<FinalDeAcao 
                && 
                myKeys.VerificaCont(KeyCont.losangulosPegos)> myKeys.VerificaCont(KeyCont.losangulosConfirmados))
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                    EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.cofreRequisitado));

                    if (myKeys.VerificaCont(KeyCont.losangulosPegos) >= FinalDeAcao)
                    {
                        new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
                        {
                            premio.SetActive(true);
                            GetComponent<SpriteRenderer>().sprite = spriteAberto;
                            Destroy(Instantiate(LosanguloManager.l.ParticulaPoeira, transform.position, Quaternion.identity), 5);

                            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
                        }, 1);
                    }
            }

            
        }
    }
}
