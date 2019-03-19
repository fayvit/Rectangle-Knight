using UnityEngine;
using System.Collections;

public class ExistenciaDoController
{
    public static bool AgendaExiste(System.Action r, MonoBehaviour m)
    {
        GameController g = GameController.g;
        if (g!=null)
            return true;
        else
        {
            m.StartCoroutine(Rotina(r));
            return false;
        }
    }

    static IEnumerator Rotina(System.Action r)
    {
        yield return new WaitForSecondsRealtime(0.15f);
        r();
    }
}