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
        }
    };
}