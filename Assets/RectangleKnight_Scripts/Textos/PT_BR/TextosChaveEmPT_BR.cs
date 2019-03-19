﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextosChaveEmPT_BR
{
    public static Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>>()
    {
        { ChaveDeTexto.bomDia,new List<string>()
        {
            "bom dia pra você",
            "bom dia ...",
            "bom dia pra você"
        }
        },
        {
            ChaveDeTexto.opcoesDeMenu, new List<string>()
            {
                "Iniciar Jogo",
                "Opções",
                "Linguagens",
                "Creditos"
            }
        },
        {
            ChaveDeTexto.certezaDeletarJogo, new List<string>()
            {
               "Tem certeza que deseja deletar o jogo {0}?"
            }
        },
        {
            ChaveDeTexto.menuDePause, new List<string>()
            {
               "Retornar ao Jogo",
               "Opções",
               "Voltar ao menu principal"
            }
        }

    };
}