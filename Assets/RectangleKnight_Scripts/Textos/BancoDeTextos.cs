using UnityEngine;
using System.Collections.Generic;

public class BancoDeTextos
{
    public static idioma linguaChave = idioma.pt_br;
    public static readonly Dictionary<idioma, Dictionary<ChaveDeTexto, List<string>>> falacoesComChave
    = new Dictionary<idioma, Dictionary<ChaveDeTexto, List<string>>>() {
        { idioma.pt_br,
            ListaDeTextosPT_BR.Txt
        },
        { idioma.en_google,
            ListaDeTextosEN_G.Txt
        }
    };

    public static readonly Dictionary<idioma, Dictionary<InterfaceTextKey, string>> textosDeInterface
        = new Dictionary<idioma, Dictionary<InterfaceTextKey, string>>() {
            {
                idioma.pt_br,
                InterfaceTextList.txt
            },
            {
                idioma.en_google,
                InterfaceTextListEN_G.txt
            }
        };



    public static void VerificaChavesFortes(idioma primeiro, idioma segundo)
    {
        if (falacoesComChave.ContainsKey(primeiro) && falacoesComChave.ContainsKey(segundo))
        {
            Dictionary<ChaveDeTexto, List<string>>.KeyCollection keys = falacoesComChave[primeiro].Keys;

            foreach (ChaveDeTexto k in keys)
            {
                if (falacoesComChave[segundo].ContainsKey(k))
                {
                    if (falacoesComChave[segundo][k].Count != falacoesComChave[primeiro][k].Count)
                    {
                        Debug.Log("As listas de mensagem no indice " + k + " tem tamanhos diferentes");
                    }
                }
                else
                {
                    Debug.Log("A lista " + segundo + " nao contem a chave: " + k);
                }
            }
        }
        else
        {
            Debug.Log("Falacoes nao contem alguma das chaves de idioma");
        }

        
    }

    public static List<string> RetornaListaDeTextoDoIdioma(ChaveDeTexto chave)
    {
        return falacoesComChave[linguaChave][chave];
    }

    public static string RetornaFraseDoIdioma(ChaveDeTexto chave)
    {
        return falacoesComChave[linguaChave][chave][0];
    }

    public static string RetornaTextoDeInterface(InterfaceTextKey chave)
    {
        return textosDeInterface[linguaChave][chave];
    }


}

public enum idioma
{
    pt_br,
    en_google
}

public enum ChaveDeTextoDosUpdates
{
    movimentacao,
    attack,
    downAttack,
    upAttack,
    pulo,
    magicRecovery,
    magicAttack,
    dash,
    downArrowJump,
    doubleJump,
    blueSword,
    greenSword,
    goldSword,
    redSword
}

public enum ChaveDeTexto
{
    bomDia,
    opcoesDeMenu,
    certezaDeletarJogo,
    menuDePause,
    androidUpdateMenu,
    androidUpdateInfo,
    emblemasTitle,
    emblemasInfo,
    frasesDeEmblema,
    nomeDeEmblemas,//deve mudar
    hexagonPentagonTips,
    conhecendoTyron,
    TyronSobreAsProfundezas,
    tyronUmCaminhoEmFrente
}


