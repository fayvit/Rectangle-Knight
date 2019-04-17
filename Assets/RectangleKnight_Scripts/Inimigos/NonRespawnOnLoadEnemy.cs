using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRespawnOnLoadEnemy : EnemyBase
{
    [SerializeField] private string ID;

    public string GetID { get=> ID; }

    protected override void Start()
    {

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.enemyDefeatedCheck, ID, gameObject));

            base.Start();
        }
    }

    

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    protected override void OnDefeated()
    {
        EventAgregator.Publish(new StandardSendGameEvent(gameObject,EventKey.requestChangeEnemyKey, ID));
        base.OnDefeated();
    }
}
