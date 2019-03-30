using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWithAttack : ActiveFalseForShift
{
    #region Inspector
    [SerializeField] private GameObject particuladaAcao = default;
    [SerializeField] private GameObject containerDasImagens =  default;
    [SerializeField] private Collider2D thisCollider = default;
    #endregion


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            Destruicao();
        }
    }

    public void Destruicao()
    {
        thisCollider.enabled = false;
        containerDasImagens.SetActive(false);
        particuladaAcao.SetActive(true);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        Destroy(gameObject, 5);
    }

}
