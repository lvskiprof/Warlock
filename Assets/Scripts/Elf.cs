using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf : Character
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
   *		This will create a random character adjusted to be the best Elf possible.
   *	Elves are much more complicated in how you want to adjust their Strength and IQ,
   *	because they need both.  Strength is more important to them to get good carrying
   *	capacity and for fighting in melee or with archery.  IQ is important for spell
   *	point bonuses, but not as much, because they get more hits than a normal Mage.
    ***/
	public void NewElf()
	{
		charClassName = "Elf";
		charClass = CharType.elf;
		uint prime;

		/***
        *       Now we need to generate the Intelligence (IQ) characteristic, which
        *   has a minimum requirement for a Mage of 9.  A Mage can adjust the IQ
        *   up by one for each reduction of strength by 2 and Wisdom by 3, but you
        *   can't reduce either characteristic below 9.
        *   
        *       It is also beneficial to have an IQ over 13 (plus 1/2 Spell Point
        *   (SP) per hit die) or 16 (+1 SP per hit die), so adjusting at least to
        *   either of those values is worth lowering the other two characteristics.
        *   
        *       An IQ of 13-14 gets an experience bonus of 5%, while 15-18 gets 10%
        *   and 19 gets 15%, so those are other good points to try and raise IQ to.
        ***/

		while (strength < 9 || intel < 9 || dex + agility < 25 || dex < 11)
		{   // Regenerate the Strength, IQ, and Dexterity for this Elf if it is less than required
			strength = dice.RollDice(3, 6);
			intel = dice.RollDice(3, 6);
			dex = dice.RollDice(3, 6);
		}   // while

		// Strength is more important for an elf, so try to get it up first in all cases
		if (strength < 13)
		{   // Try to adjust strength for the best fighting and experience
			RaiseStrengthfromWisdom(13);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (strength < 13)
		{   // Try to adjust IQ for the best fighting and experience, but don't go below 13
			RaiseStrengthfromIQ(13, 13);
		}   // if

		// Try to raise this up to get 5% experience bonus
		if (intel < 13)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromWisdom(13);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (intel < 13)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromStrength(13, 13);  // Only sacrifice Strength down to 13 for this benefit
		}   // if

		// Try to get Strength up to 15 for a chance at 10% experience bonus
		if (strength < 15)
		{   // Try to adjust strength for the best fighting and experience
			RaiseStrengthfromWisdom(15);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (strength < 15)
		{   // Try to adjust IQ for the best fighting and experience, but don't go below 13
			RaiseStrengthfromIQ(15, 13);
		}   // if

		// Now try to get IQ up to 15 for chance at 10% experience bonus
		if (intel < 15)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromWisdom(15);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (intel < 15)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromStrength(15, 15);  // Only sacrifice Strength down to 15 for this benefit
		}   // if

		// Try to get to Strength 16 for +1 Attack in melee
		if (strength < 16)
		{   // Try to adjust strength for the best fighting and experience
			RaiseStrengthfromWisdom(16);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (strength < 16)
		{   // Try to adjust IQ for the best fighting and experience, but don't go below 15
			RaiseStrengthfromIQ(16, 15);
		}   // if

		// Try to get IQ up to 16 for chance at +1 SP/hit die bonus, but don't sacrifice Strength
		if (intel < 16)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromWisdom(16);  // Only sacrifice Wisdom for this benefit
		}   // if

		// Try to get to Strength 18 for +1/die damage in melee.  We can give up +1SP/die by lowering IQ to 15
		if (strength < 18)
		{   // Try to adjust strength for the best fighting and experience
			RaiseStrengthfromWisdom(18);  // Only sacrifice Wisdom for this benefit
		}   // if

		if (strength < 18)
		{   // Try to adjust IQ for the best fighting and experience, but don't go below 15
			RaiseStrengthfromIQ(18, 15);
		}   // if

		prime = System.Math.Min(intel, strength);
		if (prime < 13)
		{   // We can set two bonus values for this
			expBonus = 0;   // No experience bonus
		}   // if
		else if (prime < 15)
			expBonus = 5;   // 5% experience bonus
		else if (prime < 19)
			expBonus = 10;  // 10% experience bonus
		else
			expBonus = 15;  // 15% experience bonus

		if (intel >= 13 && intel < 16)
			bonusSP = 5;    // You get 0.5 SP per whole hit die
		else if (intel >= 16)
			bonusSP = 10;   // You get 1.0 SP per whole hit die

		magicClass = dice.RollDice(1, 6);    // Determine a magic class
		SetGoldAtLevel();
		SetHitDice(wholeDice, fractionalDice);

		/***
		*		After this you need to determine what abilities the character knows
		*	Leaving that for a later time, when I haveabilitiess working.
		***/
	}   // newElf()

	/***
    *       This is the base creator that we need to use to access the public
    *   methods in this class.
    *       
    *       This will create a random characterthat is within a level range.
    ***/
	public Elf(uint minLevel, uint maxLevel)
	{
		/***
		*      Later on this should probably be a reverse progression up to 20,
		*  so lower levels are more common.
		***/
		level = dice.RollDice(1, (maxLevel - minLevel) + 1) + minLevel;
		NewElf();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Elf(uint minLevel, uint maxLevel)

	/***
    *       This is the base creator that we need to use to access the public
    *   methods in this class.
    *       
    *       This will create a random character within a range of levels.
    ***/
	public Elf()
	{
		/***
        *       Later on this should probably be a reverse progression up to 20,
        *   so lower levels are more common.
        ***/
		level = dice.RollDice(1, 20);
		NewElf();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Elf()
}   // class Elf