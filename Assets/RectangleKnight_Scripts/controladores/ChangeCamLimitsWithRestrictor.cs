using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamLimitsWithRestrictor : MonoBehaviour
{
    
    [SerializeField] RestritorDeCamLimits restritor=default;
    [SerializeField] private float tempoDeLerpLimits = .75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Debug.Log("restritor");
            restritor.MudeLimitantesParaTrigger(tempoDeLerpLimits);           

            //EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes, tempoDeLerpLimits));
        }
    }
}
