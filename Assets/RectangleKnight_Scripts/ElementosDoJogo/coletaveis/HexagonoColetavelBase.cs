using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexagonoColetavelBase
{
    [SerializeField] private PainelPentagonoHexagono painel = default;
    [SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private GameObject particulaDaGeometriaCompleto = default;
    [SerializeField] private GameObject particulaDaFinalizacaoDoUpdate = default;
    [SerializeField] private float tempoDaParticulaDeCompletude = 2;
    [SerializeField] private float tempoParaRetornoAposCompletude = 0.75f;

    private int repeticoesDoSomDeUpdateConcluido = 0;

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
            particulaDaGeometriaCompleto.SetActive(true);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Fire3));
            new MyInvokeMethod().InvokeNoTempoReal(FinalDaCompletude,tempoDaParticulaDeCompletude);
            new MyInvokeMethod().InvokeNoTempoReal(SomDaAcaoDeUpdate, .25f);
        }
    }

    void SomDaAcaoDeUpdate()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.XP010_System10));
        repeticoesDoSomDeUpdateConcluido++;

        if(repeticoesDoSomDeUpdateConcluido<7)
            new MyInvokeMethod().InvokeNoTempoReal(SomDaAcaoDeUpdate, .25f);

    }

    void FinalDaCompletude()
    {
        particulaDaGeometriaCompleto.SetActive(false);
        particulaDaFinalizacaoDoUpdate.SetActive(true);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.XP049_Explosion02));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.updateGeometryComplete,ePentagono));
        new MyInvokeMethod().InvokeNoTempoReal(OnCloseSecondPanel, tempoParaRetornoAposCompletude);
        new MyInvokeMethod().InvokeNoTempoReal(()=> {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.ItemImportante));
        }, 1f);
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