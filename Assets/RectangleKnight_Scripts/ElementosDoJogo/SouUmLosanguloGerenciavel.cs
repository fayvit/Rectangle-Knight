using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouUmLosanguloGerenciavel : MonoBehaviour
{
    public SpriteRenderer MySprite { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
    }

}
