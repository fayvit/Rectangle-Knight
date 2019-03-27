using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollorBarrage : DestructibleWithAttack
{
    [SerializeField] private SwordColor sColor = SwordColor.grey;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            if (GameController.g.Manager.CorDaEspadaselecionada == (int)sColor)
                base.OnTriggerEnter2D(collision);
            else
            {
                Debug.Log("criar algo para o ataque em false");
            }
        }
        
    }
}

public enum SwordColor
{
    grey,
    blue,
    green,
    gold,
    red
}