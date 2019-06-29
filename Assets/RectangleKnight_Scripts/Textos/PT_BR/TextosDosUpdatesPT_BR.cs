using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextosDosUpdatesPT_BR {
    public static Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>>()
    {
        { ChaveDeTexto.androidUpdateMenu,new List<string>()
        {
            "Movimentação",
            "Ataque",
            "Ataque para baixo",
            "Ataque para cima",
            "Pulo",
            "Recuperação magica",
            "Ataque magico",
            "Dash",
            "Flecha cadente magica",
            "Duplo pulo",
            "Espada azul",
            "Espada verde",
            "Espada dourada",
            "Espada Vermelha",
            "Movimentação"
        } },
        { ChaveDeTexto.androidUpdateInfo,new List<string>()
        {
            "Use o joystick virtual para se movimentar",
            "Para atacar use",
            "Durante o pulo pressione para baixo e o botão de ataque",
            "Pressione para cima e o botão de ataque [também durante o pulo]",
            "Para pular use ",
            "Para recuperar o HP segure ",
            "Para usar o ataque magico pressione ",
            "Para usar o Dash [também durante o pulo]  pressione ",
            "Durante o pulo, pressione para baixo e o botão de magia",
            "No ar pressione o botão de pulo novamente",
            "Você pode quebrar barreiras azuis com essa espada, selecione a espada pressionando ",
            "Você pode quebrar barreiras verdes com essa espada, selecione a espada pressionando ",
            "Você pode quebrar barreiras douradas com essa espada, selecione a espada pressionando ",
            "Você pode quebrar barreiras vermelhas com essa espada, selecione a espada pressionando ",
            "Para movimentar-se pressione"
        }
        },
        {
            ChaveDeTexto.emblemasInfo,new List<string>()
            {
                "Você pode adicionar emblemas na sua espada enquanto tiver encaixes disponiveis",
                "As moedas deixadas pelos inimigos são atraidas pelo personagem",
                "Aumenta seu potencial de ataque",
                "Aumenta o tempo de invulnerabilidade ao ser atingido por um ataque."
            }
        },
        {
            ChaveDeTexto.emblemasTitle,new List<string>()
            {
                "Encaixe Disponivel",
                "Dinheiro Magnetico",
                "Ataque Aprimorado",
                "Suspiro Longo"
            }
        },
         {
            ChaveDeTexto.frasesDeEmblema,new List<string>()
            {
                "Você colocou na espada o emblema {0}",
                "São necessários {0} espaços de emblemas para equipar {1}. Você não tem espaço suficiente",
                "Este emblema já está na espada",
                "Isso é um espaço vazio para inserir um emblema",
                "Nenhum emblema disponivel",
                "Você não tem nenhum emblema disponivel para encaixar na espada",
                "Ocupa {0} espaços"
            }
        },
         {
            ChaveDeTexto.hexagonPentagonTips,new List<string>()
            {
                "Você coletou um fragmento de Hexagono",
                "Ao completar o hexagono a barra de vida é aumentada",
                "Você coletou um fragmento de Pentagono",
                "Ao completar o pentagono a barra de magia é aumentada"
            }
        },
          {
            ChaveDeTexto.nomesItens,new List<string>()
            {
                "Anel De Integridade",
                "C. Q. D.",
                "Escada para profundezas"
            }
        },
          {
            ChaveDeTexto.descricaoDosItensNoInventario,new List<string>()
            {
                "Um anel prateado com a inscrição\"Anel de Integridade\". Será que quando o produto de nossas ações é zerado poderemos reverter o zero?",
                "Um quadrado que comumente é utilizado ao final de demonstrações de teoremas. Podemos entender seu significado como: \"Chegamos ao que queriamos demonstrar.\"",
                "Escada para profundezas"
            }
        },
          {
            ChaveDeTexto.descricaoDosItensVendidos,new List<string>()
            {
                "Um anel prateado com a inscrição\"Anel de Integridade\".\n\n Eu não sei bem o que ele quer dizer com integridade mas se você quiser compra-lo será integralmente teu",
                @"Eu já vi um quadrado desses em murais com demonstrações de teoremas. 
    Talvez você possa completar uma demonstração de teorema e colocar esse quadrado como simbolo do teu exito. Dizem que cada exito merece uma recompensa, nem que seja uma satisfação pessoal.",
                @"É bem irritante pular naquela fenda para a garganta das profundezas e não poder voltar não é?

Seus problemas acabaram!!

Comprando essa escada você poderá subir de volta a partir da fenda.",
                "Encontrei esse selo no meio das minhas coisas. Não tenho certeza sobre o valor real dele ou utilidade então resolvi vender. Se quise-lo, é teu por uma modesta quantia.",
                @"Você tem visto muitas de suas moedas rolarem para lugares onde você não consegue pegar?

Seus problemas acabaram!!

Esse emblema faz com que as moedas sejam atraidas por você.",
                "Com esse emblema na sua espada o tempo que você fica invencivel ao tomar dano aumenta. Pense bem... É ou não é bom ter um suspiro mais longo?",
                "Muitas figuras geometricas adorariam ter um hexagono para sentir-se melhor. Bem... Eu não tenho um hexagono completo, mas esse fragmento deve ser um começo",
                "Muitas figuras geometricas adorariam ter um pentagono para sentir-se mais poderosas. Bem... Eu não tenho um pentagono completo, mas esse fragmento deve ser um começo.",


            }
        },
          {
            ChaveDeTexto.nomeParaItensVendidos,new List<string>()
            {
                "Anel de Integridade",
                "C. Q. D.",
                "Escada para as profundezas",
                "Selo Positivista do Amor",
                "Emblema do dinheiro magnético",
                "Emblema do Suspiro Longo",
                "Fragmento de Hexagono",
                "Fragmento de Pentagono"
            }
        },
          {
            ChaveDeTexto.frasesParaTutoPlacas,new List<string>()
            {
                "Você pode recuperar seus pontos de vida segurando",
                "Recuperar pontos de vida custa pontos de magia. Você recupera pontos de magia atacando os inimigos",
                "Você coletou um encaixe para emblema",
                "Agora você tem um espaço a mais para colocar emblemas na espada",
                "Você pode colocar emblemas na sua espada enquanto está num checkPoint",
                "Quando em um checkpoint utilize o menu de pause para colocar emblemas na espada"
            }
        },
        {
            ChaveDeTexto.updateSetaSombria, new List<string>()
            {
                "Você aprendeu a usar a Seta Magica",
                "Usar a seta magica custa pontos de magia. Você recupera pontos de magia atacando inimigos",
                "Para usar a seta magica pressione",
                "Pressionando rapidamente o botão de magia você disparará uma seta magica."
            }
        },
        {
            ChaveDeTexto.updateBlueSword, new List<string>()
            {
                "Você coletou a espada Azul",
                "Com a Espada Azul você pode quebrar barreiras azuis",
                "Alterna a cor da Espada",
                "Você pode alternar a cor da sua espada pressionando os botões coloridos"
            }
        }
    };
}