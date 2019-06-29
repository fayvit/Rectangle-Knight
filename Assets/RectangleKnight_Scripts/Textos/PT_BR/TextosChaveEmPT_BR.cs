using System.Collections;
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
        },
        {
            ChaveDeTexto.nomesParaViagensDeCapsula, new List<string>()
            {
               "Garganta das profundezas",
               "Acampamento dos Rejeitados",
               "Aquifero do buscador"
            }
        },
        {
            ChaveDeTexto.textosDaLojaDeHerika, new List<string>()
            {
               "Estoque vazio",
               "Parece que você comprou tudo que tinha na loja. Volte mais tarde, talvez tenha novas mercadorias para vender"
            }
        },
        {
            ChaveDeTexto.complementosDoMenuDePause, new List<string>()
            {
               "Inventario Vazio",
               "Você não possui nenhum item no seu inventário no momento"
            }
        }

    };
}