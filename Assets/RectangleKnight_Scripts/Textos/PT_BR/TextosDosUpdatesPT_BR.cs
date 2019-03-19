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
            "Espada Vermelha"
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
            "Você pode quebrar barreiras vermelhas com essa espada, selecione a espada pressionando "
            /*"movimentação,
             * attack,
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
    redSword"*/
        }
        }
    };
}