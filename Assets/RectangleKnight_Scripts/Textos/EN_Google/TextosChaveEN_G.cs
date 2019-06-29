using System.Collections.Generic;

public class TextosChaveEN_G
{
    public static Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>>()
    {
        { ChaveDeTexto.bomDia,new List<string>()
        {
            "Goog Morning...",
            "good morning for you...",
            "Goog Morning..."
        }
        },
        {
            ChaveDeTexto.opcoesDeMenu, new List<string>()
            {
                "Start game",
                "Options",
                "Languages",
                "Credits"
            }
        },
        {
            ChaveDeTexto.certezaDeletarJogo, new List<string>()
            {
               "Are you sure you want to delete the game {0}?"
            }
        },
        {
            ChaveDeTexto.menuDePause, new List<string>()
            {
               "Return to Game",
               "Options",
               "Return to main menu"
            }
        },
        {
            ChaveDeTexto.nomesParaViagensDeCapsula, new List<string>()
            {
               "Throat of the deep",
               "Rejected Camp",
               "Aquifer of the Seeker"
            }
        },
        {
            ChaveDeTexto.textosDaLojaDeHerika, new List<string>()
            {
               "Empty stock",
               "Looks like you bought everything you had in the store, come back later, maybe have new goods to sell"
            }
        },
        {
            ChaveDeTexto.complementosDoMenuDePause, new List<string>()
            {
               "Empty Inventory",
               "You do not currently have any items in your inventory"
            }
        }
    };
}