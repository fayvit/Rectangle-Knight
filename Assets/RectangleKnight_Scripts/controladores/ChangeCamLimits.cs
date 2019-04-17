using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamLimits : MonoBehaviour
{
    [SerializeField] private DadosDeCena.LimitantesDaCena limitantes;
    [SerializeField] private NomesCenas usarLimitanteDaCena = NomesCenas.nula;
    [SerializeField] private float tempoDeLerpLimits = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(usarLimitanteDaCena!=NomesCenas.nula)
                limitantes = GlobalController.g.SceneDates.GetCurrentSceneDates().limitantes;

            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes,tempoDeLerpLimits));
        }
    }
}
