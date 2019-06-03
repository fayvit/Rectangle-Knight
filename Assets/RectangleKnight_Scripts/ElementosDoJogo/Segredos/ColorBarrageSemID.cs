using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBarrageSemID : DestructibleWithAttack
{
    [SerializeField] private SwordColor sColor = SwordColor.grey;


    private const float forcaDeRepulsa = 850;
    private const float tempoNaRepulsao = .15f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            if (GameController.g.Manager.CorDaEspadaselecionada == (int)sColor)
            {
                if (collision.tag == "attackCollisor")
                {
                    DestruicaoSemID();
                }
            }
            else if (collision.name != "magicAttack")
            {
                SoundOnAttack.SoundAnimationAndRepulse(transform, forcaDeRepulsa, tempoNaRepulsao, collision.transform.position);

            }
        }

    }
}