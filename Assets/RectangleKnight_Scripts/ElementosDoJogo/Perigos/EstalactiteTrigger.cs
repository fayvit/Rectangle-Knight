using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstalactiteTrigger : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] EstalactiteQueCai queCai;
#pragma warning restore 0649
    private bool iniciou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&!iniciou)
        {
            iniciou = true;
            queCai.Iniciar();
        }

    }
}
