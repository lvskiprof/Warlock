using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : Character
{
	static readonly int[] wholeDice =
				//  0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,
		new int[] { 0, 1, 1, 2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 9,10,10,11,11,
				// 19,20,21,22,23,24,25,26,27,28,29,30
				   12,12,12,13,13,13,14,14,14,15,15,15};
	static readonly int[] fractionalDice =
				//  0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,
		new int[] { 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 5, 0, 5,
				// 19,20,21,22,23,24,25,26,27,28,29,30
					0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2};
	/***
	*		This method calls GetCharacterInfo() from the parent Character class and adds
	*	items specific to the Dwarf class.
	***/
	public new string GetCharacterInfo()
	{
		return base.GetCharacterInfo() + "\n" +
			   "";
	}   // GetCharacterInfo()

	/***
    *      This will create a random character adjusted to be the best Dwarf
    *  possible.
    ***/
	public void NewDwarf()
	{
		charClassName = "Dwarf";
		charClass = CharType.dwarf;

		/***
        *      Now we need to generate the Strength & Constitution characteristics, which
        *  have a minimum requirement for a Dawrf of 15.  A Mage can adjust the IQ
        *  up by one for each reduction of strength by 2 and Wisdom by 3, but you
        *  can't reduce either characteristic below 9.
        *  
        *      It is also beneficial to have an IQ over 13 (plus 1/2 Spell Point
        *  (SP) per hit die) or 16 (+1 SP per hit die), so adjusting at least to
        *  either of those values is worth lowering the other two characteristics.
        *  
        *      An IQ of 13-14 gets an experience bonus of 5%, while 15-18 gets 10%
        *  and 19 gets 15%, so those are other good points to try and raise IQ to.
        ***/

		while (strength < 15)
		{   // Regenerate the strength for this Dwarf if it is less than 15
			strength = dice.RollDice(3, 6);
		}   // while

		while (constitution < 15)
		{   // Regenerate the constitution for this Dwarf if it is less than 15
			constitution = dice.RollDice(3, 6);
		}   // while

		if (strength < 15)
			expBonus = 5;   // 5% experience bonus
		else if (strength < 19)
			expBonus = 10;  // 10% experience bonus
		else
			expBonus = 15;  // 15% experience bonus

		SetGoldAtLevel();
		SetHitDice(wholeDice, fractionalDice);

		/***
		*		After this you need to determine what abilities the character knows
		*	Leaving that for a later time, when I have abilitiess working.
		***/
	}   // newDwarf()

	/***
    *      This is the base creator that we need to use to access the public
    *  methods in this class.
    *      
    *      This will create a random characterthat is within a level range.
    ***/
	public Dwarf(uint minLevel, uint maxLevel)
	{
		/***
		*      Later on this should probably be a reverse progression up to 20,
		*  so lower levels are more common.
		***/
		level = dice.RollDice(1, (maxLevel - minLevel) + 1) + minLevel;
		NewDwarf();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Dwarf(uint minLevel, uint maxLevel)

	/***
    *      This is the base creator that we need to use to access the public
    *  methods in this class.
    *      
    *      This will create a random character within a range of levels.
    ***/
	public Dwarf()
	{
		/***
        *      Later on this should probably be a reverse progression up to 20,
        *  so lower levels are more common.
        ***/
		level = dice.RollDice(1, 20);
		NewDwarf();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Dwarf()
}   // class Dwarf