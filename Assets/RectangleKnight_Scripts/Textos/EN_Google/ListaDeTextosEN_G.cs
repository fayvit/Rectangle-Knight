using UnityEngine;
using System.Collections.Generic;

public class ListaDeTextosEN_G
{
    static Dictionary<ChaveDeTexto, List<string>> txt;

    public static Dictionary<ChaveDeTexto, List<string>> Txt
    {
        get
        {
            if (txt == null)
            {
                txt = new Dictionary<ChaveDeTexto, List<string>>();

                ColocaTextos(ref txt, TextosChaveEN_G.txt);
                ColocaTextos(ref txt, TextosDosUpdatesEN_G.txt);
                ColocaTextos(ref txt, ConversasDaArea1EN_G.txt);
                ColocaTextos(ref txt, ConversasDaArea2EN_G.txt);
                ColocaTextos(ref txt, LocalSceneNamesEN_G.txt);
                /*
                ColocaTextos(ref txt, TextosDeBarreirasPT_BR.txt);
                ColocaTextos(ref txt, TextosDaCavernaInicialPT_BR.txt);
                ColocaTextos(ref txt, TextosDeKatidsPT_BR.txt);
                ColocaTextos(ref txt, TextosDeMarjanPT_BR.txt);
                ColocaTextos(ref txt, TextosDeInfoPT_BR.txt);
                */
            }

            return txt;
        }
    }

    static void ColocaTextos(ref Dictionary<ChaveDeTexto, List<string>> retorno, Dictionary<ChaveDeTexto, List<string>> inserir)
    {
        foreach (ChaveDeTexto k in inserir.Keys)
        {
            retorno[k] = inserir[k];
        }
    }

}