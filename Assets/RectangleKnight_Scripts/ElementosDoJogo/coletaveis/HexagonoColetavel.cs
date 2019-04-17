using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonoColetavel : AtivadorDeBotao
{
    #region inspector
    [SerializeField] private PainelPentagonoHexagono painel = default;
    [SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private bool ePentagono = false;
    [SerializeField] private string ID;
    #endregion

    public struct DadosDaGeometriaColetavel
    {
        public string ID;
        public bool ePentagono;
        public float velocidadeNaQuedaDaMusica;
    }

    private void Start()
    {
        SempreEstaNoTrigger();

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, gameObject));
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
               // Coletou();
            }
        }
    }

    void Coletou()
    {
        Time.timeScale = 0;

        painel.ConstroiPainelDosPentagonosOuHexagonos(OnCloseFirstPanel,
            ePentagono ? PainelPentagonoHexagono.Forma.pentagono :
            PainelPentagonoHexagono.Forma.hexagono);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.getUpdateGeometry,
            new DadosDaGeometriaColetavel() { ID = ID,ePentagono = ePentagono,velocidadeNaQuedaDaMusica = 2.5f}));

        /*
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(ePentagono ? EventKey.getPentagon : EventKey.getHexagon, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic, 2.5f));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
        */

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Collider2D>());
    }

    IEnumerator PainelAoFimDoQuadro()
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.addUpdateGeometry));
        new MyInvokeMethod().InvokeNoTempoReal(() =>
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.vinhetinhaDoSite));
        }, .25f);

        painel.ConstroiPainelDosPentagonosOuHexagonos(OnCloseSecondPanel, 
            ePentagono? PainelPentagonoHexagono.Forma.pentagono : PainelPentagonoHexagono.Forma.hexagono);
    }

    void OnCloseFirstPanel()
    {
        particulaDaAcao.SetActive(true);

        if( (GameController.g.Manager.Dados.PartesDeHexagonoObtidas < 6 && !ePentagono)
            || (GameController.g.Manager.Dados.PartesDePentagonosObtidas < 6 && ePentagono))
        {
            StartCoroutine(PainelAoFimDoQuadro());
        }
        else
        {
            Debug.LogError("falta fazer a parte do hexagono completo");
            Destroy(gameObject);
        }
    }

    void OnCloseSecondPanel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
        EventAgregator.Publish(EventKey.restartMusic, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Book1));
        Destroy(gameObject);
    }

    public override void FuncaoDoBotao()
    {
        btn.SetActive(false);
        Coletou();
    }
}
