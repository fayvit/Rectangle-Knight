using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonoColetavel : AtivadorDeBotao
{
    #region inspector
    //[SerializeField] private PainelPentagonoHexagono painel = default;
    //[SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private bool ePentagono = false;
    [SerializeField] private string ID;
    #endregion

    [SerializeField] private HexagonoColetavelBase hexBase = default;

    private void Start()
    {
        SempreEstaNoTrigger();

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, gameObject));
        }

        EventAgregator.AddListener(EventKey.hexCloseSecondPanel,OnHexCloseSecondPanel);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.hexCloseSecondPanel, OnHexCloseSecondPanel);
    }

    void OnHexCloseSecondPanel(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if ((string)ssge.MyObject[0] == ID)
        {
            Time.timeScale = 1;
            EventAgregator.Publish(EventKey.fechouPainelSuspenso);
            MonoBehaviour.Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
               // Coletou();
            }
        }
    }*/

    public override void FuncaoDoBotao()
    {
        btn.SetActive(false);
        hexBase.Coletou(ePentagono,ID);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Collider2D>());
    }
}
