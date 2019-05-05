using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDaLojaDeHerika : AtivadorDaLoja
{
    [SerializeField] private LojaDeHerika lojaDaHerika = default;
    protected override void Start()
    {
        EssaLoja = lojaDaHerika;
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
