using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncaixeColetavel : ColetavelBasse
{
    protected override void AcaoEspecificaDaColeta()
    {
        EventAgregator.Publish(EventKey.getNotch, null);
    }
}
