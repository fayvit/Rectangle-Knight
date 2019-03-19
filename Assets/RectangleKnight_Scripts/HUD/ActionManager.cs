using UnityEngine;
using System.Collections;

public static class ActionManager
{
    static AtivadorDeBotao visualizado;
    static System.Action acao;

    public static bool useiCancel = false;

    private static bool esteQuadro = false;


    public static bool PodeVisualizarEste(AtivadorDeBotao Tt)
    {
        Transform T = Tt.transform;

        bool pode = false;
        if (visualizado != null)
        {
            if (Vector3.Distance(GameController.g.Manager.transform.position, T.position)
                <
                Vector3.Distance(GameController.g.Manager.transform.position, visualizado.transform.position))
            {
                pode = true;
                visualizado = Tt;
                acao = null;
            }

            if (visualizado == Tt)
                pode = true;
        }
        else
        {
            pode = true;
            visualizado = Tt;
            acao = null;
        }
        return pode;
    }

    public static bool TransformDeActionE(Transform T)
    {
        return T == visualizado;
    }

    public static void ModificarAcao(AtivadorDeBotao T, System.Action acao)
    {
        visualizado = T;
        ActionManager.acao = acao;
    }

    
    public static void VerificaAcao()
    {
        if (!esteQuadro)
        {
            AgendaEsseQuadro();
            if (visualizado != null)
                if (visualizado.Btn.activeSelf)
                {
                    if (acao != null)
                    {
                        acao();
                    }
                    else
                    {
                        visualizado.FuncaoDoBotao();
                    }
                }
        }
    }

    public static bool ButtonUp(int n, Controlador c)
    {
        bool press = CommandReader.ButtonUp(n, c);
        if (!esteQuadro && press)
        {

            AgendaEsseQuadro();
            return true;
        }
        else return false;
    }

    
    static void AgendaEsseQuadro()
    {
        esteQuadro = true;
        GlobalController.g.StartCoroutine(VoltaQuadro());
    }

    static IEnumerator VoltaQuadro()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        esteQuadro = false;
    }
}
