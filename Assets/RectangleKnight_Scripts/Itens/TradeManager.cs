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
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getUpdateGeometry,
                    new HexagonoColetavel.DadosDaGeometriaColetavel()
                    {
                        ID="sempreTrue",
                        ePentagono = n==NomeMercadoria.fragmentoDePentagono,
                        velocidadeNaQuedaDaMusica = 2.5f
                    }
                    ));
            break;
            case NomeMercadoria.escadaParaProfundezas:
            break;
            case NomeMercadoria.SeloPositivistaDoAmor:
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getStamp, MercadoriaToSeloPositivistas(n)));
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