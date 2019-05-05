using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexagonoColetavelBase
{
    [SerializeField] private PainelPentagonoHexagono painel = default;
    [SerializeField] private GameObject particulaDaAcao = default;

    private bool ePentagono;    
    private string ID = "";

    public struct DadosDaGeometriaColetavel
    {
        public string ID;
        public bool ePentagono;
        public float velocidadeNaQuedaDaMusica;
    }

    void OnCloseFirstPanel()
    {
        particulaDaAcao.SetActive(true);

        if ((GameController.g.Manager.Dados.PartesDeHexagonoObtidas < 6 && !ePentagono)
            || (GameController.g.Manager.Dados.PartesDePentagonosObtidas < 5 && ePentagono))
        {
            GameController.g.StartCoroutine(PainelAoFimDoQuadro());
        }
        else
        {
            Debug.LogError("falta fazer a parte do hexagono completo");
          //  MonoBehaviour.Destroy(G);
        }
    }

    void OnCloseSecondPanel()
    {
        
        EventAgregator.Publish(EventKey.restartMusic);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Book1));
        

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.hexCloseSecondPanel,ID));
    }

    public void Coletou(bool ePentagono,string ID)
    {
        //this.painel = painel;
        this.ePentagono = ePentagono;
        this.ID = ID;
        //this.particulaDaAcao = particulaDaAcao;

        Time.timeScale = 0;

        painel.ConstroiPainelDosPentagonosOuHexagonos(OnCloseFirstPanel,
            ePentagono ? PainelPentagonoHexagono.Forma.pentagono :
            PainelPentagonoHexagono.Forma.hexagono);

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.getUpdateGeometry,
            new DadosDaGeometriaColetavel() { ID = ID, ePentagono = ePentagono, velocidadeNaQuedaDaMusica = 2.5f }));

        /*
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(ePentagono ? EventKey.getPentagon : EventKey.getHexagon, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.stopMusic, 2.5f));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
        */

        
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
            ePentagono ? PainelPentagonoHexagono.Forma.pentagono : PainelPentagonoHexagono.Forma.hexagono);
    }
}