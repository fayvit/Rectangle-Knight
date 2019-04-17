using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinhoDeMoedas : ActiveFalseForShift
{

    [SerializeField] private int numMoedas = 20;
    [SerializeField] private int numHits = 3;

    #region inspector
    [SerializeField] private GameObject particulaDaFinalizacao = default;
    #endregion


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KeyVar myKeys = GameController.g.MyKeys;

        if (collision.tag == "attackCollisor")
        {
            Debug.Log(myKeys.VerificaAutoCont(ID)+" os hits");
            myKeys.SomaAutoCont(ID, 1);
            int moedasAgora;
            bool foi = myKeys.VerificaAutoCont(ID) < numHits;

            moedasAgora = numMoedas / (numHits + 1);

            if (!foi)
                moedasAgora = numMoedas - (numHits - 1) * moedasAgora;

        
            SpawnMoedas.Spawn(transform.position,moedasAgora);
            new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, "Break"));
            },.3f);

            if (!foi)
            {
                myKeys.MudaAutoShift(ID, true);
                particulaDaFinalizacao.SetActive(true);
                Destroy(GetComponent <Collider2D>());
                Destroy(GetComponent<SpriteRenderer>());
                Destroy(gameObject,5);
            }

            
        }
    }
}
