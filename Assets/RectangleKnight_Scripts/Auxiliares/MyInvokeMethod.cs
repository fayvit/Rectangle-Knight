using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInvokeMethod
{
    private System.Action acao;

    public void InvokeAoFimDoQuadro(System.Action acao)
    {
        this.acao = acao;
        GameController.g.StartCoroutine(EndFrameInvoke());
    }

    public void InvokeNoTempoDeJogo(System.Action acao, float tempo)
    {
        this.acao = acao;
        GameController.g.StartCoroutine(TimedInvoke(tempo));
    }

    public void InvokeNoTempoDeJogo(GameObject G,System.Action acao, float tempo)
    {
        this.acao = acao;
        GameController.g.StartCoroutine(TimedInvokeComObject(G,tempo));
    }

    public void InvokeNoTempoReal(System.Action acao, float tempo)
    {
        this.acao = acao;
        GameController.g.StartCoroutine(RealTimeTimedInvoke(tempo));
    }

    IEnumerator TimedInvokeComObject(GameObject G,float time)
    {
        yield return new WaitForSeconds(time);

        if (G != null)
        {            
            Acao();
        }
    }

    IEnumerator TimedInvoke(float time)
    {
        yield return new WaitForSeconds(time);
        Acao();

        
    }

    IEnumerator RealTimeTimedInvoke(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Acao();
    }

    IEnumerator EndFrameInvoke()
    {
        yield return new WaitForEndOfFrame();

        Acao();
    }

    void Acao()
    {
        acao();
        acao = null;
    }
}
