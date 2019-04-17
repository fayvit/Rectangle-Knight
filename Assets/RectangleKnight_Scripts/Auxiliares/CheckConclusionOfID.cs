using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConclusionOfID : MonoBehaviour
{
    [SerializeField] private string ID;

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

    protected void RequestChangeKey()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey,ID));
    }
}
