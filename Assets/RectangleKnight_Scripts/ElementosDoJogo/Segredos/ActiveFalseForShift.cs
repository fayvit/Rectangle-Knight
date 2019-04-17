using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFalseForShift : MonoBehaviour
{
    [SerializeField] protected string ID;

    protected virtual void Start()
    {
        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, gameObject));
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    public static void StaticStart(System.Action a,MonoBehaviour m,string ID)
    {
        if (ExistenciaDoController.AgendaExiste(a, m))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, m.gameObject));
        }
    }
}
