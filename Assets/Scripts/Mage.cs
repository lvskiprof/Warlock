using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [SerializeField]
    Characteristics stats = new Characteristics();   //  Characteristics of the character.

    /***
     *      This is the base creator that we need to use to access the public methods in this class.
     *      
     *      This will create a random character of a random level.
    ***/
    public Mage()
    {   //Instance creator
        AdventureGame game = new AdventureGame();

        stats.level = game.RollDice(1, 20); // Change later to be more of a progression that favors lower levels

        /***
         *      Roll all characteristics that are not required for a Mage first.
        ***/
        stats.strength = game.RollDice(3, 6);
        stats.wisdom = game.RollDice(3, 6);
        stats.strength = game.RollDice(3, 6);
        stats.strength = game.RollDice(3, 6);
        stats.strength = game.RollDice(3, 6);
        stats.strength = game.RollDice(3, 6);
        stats.strength = game.RollDice(3, 6);
    }   // Mage()
}
