using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TradeManager
{
    public static void OnBuy(NomeMercadoria n,int quantidade = 1)
    {
        switch (n)
        {
            case NomeMercadoria.anelDeIntegridade:
            case NomeMercadoria.CQD:
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getItem,MercadoriaToItem(n),quantidade));
            break;
            case NomeMercadoria.dinheiroMagnetico:
            case NomeMercadoria.suspiroLongo:
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getEmblem, MercadoriaToEmblema(n)));
            break;
            case NomeMercadoria.fragmentoDeHexagono:
            case NomeMercadoria.fragmentoDePentagono:
                new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
                {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.buyUpdateGeometry, n == NomeMercadoria.fragmentoDePentagono));
                });
            break;
            case NomeMercadoria.escadaParaProfundezas:
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, KeyShift.escadaDasProfundezas));
            break;
            case NomeMercadoria.SeloPositivistaDoAmor:
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getStamp, MercadoriaToSeloPositivistas(n)));
            break;
            
        }

        switch (n)
        {
            case NomeMercadoria.anelDeIntegridade:
            case NomeMercadoria.CQD:
            case NomeMercadoria.dinheiroMagnetico:
            case NomeMercadoria.suspiroLongo:
            case NomeMercadoria.escadaParaProfundezas:
            case NomeMercadoria.SeloPositivistaDoAmor:
                new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
                {
                    EventAgregator.Publish(EventKey.compraConcluida);
                });
            break;
        }
    }

    public static void OnSell(NomeMercadoria n)
    {

    }

    static NomeItem MercadoriaToItem(NomeMercadoria n)
    {
        return StringParaEnum.ObterEnum<NomeItem>(n.ToString());
    }

    static NomesEmblemas MercadoriaToEmblema(NomeMercadoria n)
    {
        return StringParaEnum.ObterEnum<NomesEmblemas>(n.ToString());
    }

    static SeloPositivista.TipoSelo MercadoriaToSeloPositivistas(NomeMercadoria n)
    {
        SeloPositivista.TipoSelo s = SeloPositivista.TipoSelo.amor;
        switch (n)
        {
            case NomeMercadoria.SeloPositivistaDoAmor:
                s = SeloPositivista.TipoSelo.amor;
            break;/*
            case NomeMercadoria.SeloPositivistaDoAmor:
                s = SeloPositivista.TipoSelo.progresso;
            break;
            case NomeMercadoria.SeloPositivistaDoAmor:
                s = SeloPositivista.TipoSelo.ordem;
            break;*/
        }

        return s;
    }
}