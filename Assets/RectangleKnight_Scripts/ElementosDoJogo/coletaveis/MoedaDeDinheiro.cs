using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoedaDeDinheiro : MonoBehaviour
{
    [SerializeField] private int valor = 1;
#pragma warning disable 0649
    [SerializeField] private GameObject particulaDaAcao;
#pragma warning restore 0649

    private void Start()
    {
        if (GameController.g.MyKeys.VerificaAutoShift("equiped_" + NomesEmblemas.dinheiroMagnetico.ToString()))
        {
            new MyInvokeMethod().InvokeNoTempoDeJogo(
                () => {
                    if(this!=null)
                        transform.parent.gameObject.AddComponent<MagnetismoParaMoeda>();
                }, .75f
                );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            particulaDaAcao.SetActive(true);
            Destroy(Instantiate(particulaDaAcao, transform.position, Quaternion.identity),5);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.getCoin, valor));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Moeda));
            Destroy(transform.parent.gameObject);
        }
    }
}
