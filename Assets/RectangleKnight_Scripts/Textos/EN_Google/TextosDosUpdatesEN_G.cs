using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextosDosUpdatesEN_G
{
    public static Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>>()
    {
        { ChaveDeTexto.androidUpdateMenu,new List<string>()
        {
            "Movement",
            "Attack",
            "Attack down",
            "Attack Up",
            "Jump",
            "Magic recovery",
            "Magic Attack",
            "Dash",
            "Magic falling arrow",
            "Double jump",
            "Blue Sword",
            "Green Sword",
            "Golden Sword",
            "Red Sword",
            "Movement"
        } },
        { ChaveDeTexto.androidUpdateInfo,new List<string>()
        {
            "Use the virtual joystick to move",
            "To attack use",
            "During the jump press down and the attack button",
            "Press up and the attack button [also during the jump]",
            "To Jump use",
            "To recover the HP hold",
            "To use magic attack press",
            "To use Dash [also during the jump] press",
            "During the jump, press down and the magic button",
            "In the air press the jump button again",
            "You can break blue barriers with this sword, select the sword by pressing",
            "You can break green barriers with this sword, select the sword by pressing",
            "You can break gold barriers with this sword, select the sword by pressing",
            "You can break red barriers with this sword, select the sword by pressing",
            "To move press"
        }
        },
        {
            ChaveDeTexto.emblemasInfo,new List<string>()
            {
                "You can add emblems to your sword while you have available slots",
                "The coins left by the enemies are attracted by the character",
                "Increases your attack potential",
                "Increases invulnerability time by being hit by an attack."
            }
        },
        {
            ChaveDeTexto.emblemasTitle,new List<string>()
            {
                "Fitting Available",
                "Magnetic Money",
                "Enhanced Attack",
                "Long Sigh"
            }
        },
         {
            ChaveDeTexto.frasesDeEmblema,new List<string>()
            {
               "You have put the emblem {0} on the sword",
                "You need {0} emblem spaces to equip {1}. You do not have enough space",
                "This emblem is already on the sword",
                "This is an empty space to insert an emblem",
                "No emblem available",
                "You do not have any badge available to fit the sword",
                "Occupy {0} spaces"
            }
        },
         {
            ChaveDeTexto.hexagonPentagonTips,new List<string>()
            {
                "You collected a fragment of Hexagon",
                "When completing the hexagon the life bar is increased",
                "You collected a fragment of Pentagon",
                "When completing the pentagon the magic bar is increased"
            }
        },
          {
            ChaveDeTexto.nomesItens,new List<string>()
            {
                "Ring of Integrity",
                "Q. E. D.",
                "Ladder to Depths"
            }
        },
          {
            ChaveDeTexto.descricaoDosItensNoInventario,new List<string>()
            {
               "A silver ring with the inscription \" Integrity Ring \". When the product of our actions is zeroed can we reverse the zero?",
                "A square that is commonly used at the end of demonstrations of theorems We can understand its meaning as: \" quod erat demonstrandum. \" ",
                "Ladder to Depths"
            }
        },
          {
            ChaveDeTexto.descricaoDosItensVendidos,new List<string>()
            {
                "A silver ring with the inscription \" Ring of Integrity \". \n \n I do not quite know what he means by integrity but if you want to buy it it will be entirely yours",
                @"I have seen a square of these in murals with demonstrations of theorems.
    Maybe you can complete a proof of theorem and put that square as a symbol of your success. They say that every success deserves a reward, even a personal satisfaction. ",
                @"It's pretty annoying to jump in that crevice to the throat from the depths and not be able to return, is not it?

Your problems are over!!

By buying this ladder you can climb back up from the crevice. ",
                "I found this stamp in the middle of my things, I'm not sure about the real value of it or usefulness so I decided to sell it, if I wanted it, it's yours for a modest amount.",
                @"Have you seen many of your coins roll into places you can not pick up?

Your problems are over!!

This emblem causes the coins to be attracted to you. ",
                "With this emblem on your sword, the time you are invincible as you take damage increases. Think well ... Is not it good to have a longer sigh?",
                "Many geometric figures would love to have a hex to feel better. Well ... I do not have a full hexagon, but that fragment should be a start",
                "Many geometric figures would love to have a pentagon to feel more powerful. Well ... I do not have a full pentagon, but that fragment should be a start.",


            }
        },
          {
            ChaveDeTexto.nomeParaItensVendidos,new List<string>()
            {
                "Ring of Integrity",
                "Q. E. D.",
                "Ladder to the Depths",
                "Positivist Seal of Love",
                "Magnetic Money Emblem",
                "Long Sigh Emblem",
                "Fragment of Hexagono",
                "Fragment of Pentagon"
            }
        },
          {
            ChaveDeTexto.frasesParaTutoPlacas,new List<string>()
            {
                "You can regain your life holding points",
                "Retrieving hit points costs magic points. You recover magic points by attacking your enemies",
                "You have collected an attachment for emblem",
                "Now you have more space to put emblems on the sword",
                "You can put badges on your sword while you're at a checkpoint",
                "When in a checkpoint use the pause menu to put emblems on the sword"
            }
        },
        {
            ChaveDeTexto.updateSetaSombria, new List<string>()
            {
               "You learned how to use the Magic Arrow",
                "Using the magic arrow costs magic points. You recover magic points by attacking enemies",
                "To use the magic arrow press",
                "By quickly pressing the magic button you will fire a magic arrow."
            }
        }
    };
}
