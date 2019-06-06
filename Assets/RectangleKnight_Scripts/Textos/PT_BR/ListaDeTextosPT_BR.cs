using UnityEngine;
using System.Collections.Generic;

public class ListaDeTextosPT_BR
{
    //static Dictionary<ChaveDeTexto, List<string>> txt;

    public static Dictionary<ChaveDeTexto, List<string>> Txt
    {
        get {
            
            Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>>();

            ColocaTextos(ref txt, TextosChaveEmPT_BR.txt);
            ColocaTextos(ref txt, TextosDosUpdatesPT_BR.txt);
            ColocaTextos(ref txt, ConversasDaArea1PT_BR.txt);
            ColocaTextos(ref txt, ConversasDaArea2PT_BR.txt);
            ColocaTextos(ref txt, LocalSceneNamesPT_BR.txt);

            return txt;
        }
    }

    static void ColocaTextos(ref Dictionary<ChaveDeTexto, List<string>>  retorno, Dictionary<ChaveDeTexto, List<string>> inserir)
    {
        foreach (ChaveDeTexto k in inserir.Keys)
        {
            retorno[k] = inserir[k];
        }
    }
    
}