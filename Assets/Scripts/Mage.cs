using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
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
	*	items specific to the Mage class.
	***/
	public new string GetCharacterInfo()
	{
		string fractionSP;

		if (bonusSP == 5)
			fractionSP = "+0.5";
		else if (bonusSP == 10)
			fractionSP = "+1";
		else
			fractionSP = "+0;";

		return base.GetCharacterInfo() + "\n" +
			   "Spell points: " + spellPoints.wholeDice + "." + spellPoints.fractionDie.ToString() +
			   " P bonus: " + fractionSP + "SP/die" +
			   " Magic class:: " + magicClass + "\n" +
			   "";
	}   // GetCharacterInfo()

	/***
    *       This will create a random character adjusted to be the best Mage
    *   possible.
    ***/
	public void NewMage()
	{
		charClassName = "Mage";
		charClass = CharType.mage;

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

		while (intel < 9)
		{   // Regenerate the IQ for this Mage if it is less than 9
			intel = dice.RollDice(3, 6);
		}	// while

		if (intel < 16)
		{   // Try to adjust IQ for the best SP per hit die
			RaiseIQfromWisdom(16);
			if (intel < 16)
				RaiseIQfromStrength(16);    // Try adjusting from Strength if needed
		}   // if

		if (intel < 19)
		{   // Try to adjust IQ for the best experience bonus
			RaiseIQfromWisdom(19);  // Only sacrifice Wisdom for this minor benefit
		}   // if

		if (intel < 13)
		{   // We can set two bonus values for this
			expBonus = 0;   // No experience bonus
			bonusSP = 0;	// No SP bonus
		}	// if
		else if (intel < 15)
			expBonus = 5;   // 5% experience bonus
		else if (intel < 19)
			expBonus = 10;  // 10% experience bonus
		else
			expBonus = 15;  // 15% experience bonus

		if (intel >= 13 && intel < 16)
			bonusSP = 5;    // You get 0.5 SP per whole hit die
		else if (intel >= 16)
			bonusSP = 10;	// You get 1.0 SP per whole hit die

		magicClass = (uint)Random.Range(1.0f, 6.0f);    // Determine a magic class
		SetGoldAtLevel();
		SetHitDice(wholeDice, fractionalDice);
		spellPointMargin = deathMargin;
		spellPoints = hits;
		spellPoints.wholeDice += ((hitDice[level].wholeDice* bonusSP) / 10);
		if (bonusSP == 5 && (hitDice[level].whole & 1) == 1)
		{   // For odd whole hit dice this character gets a 1/2 SP
			spellPoints.fractionDie += 5;
			if (spellPoints.fractionDie == 10)
			{   // We rounded up to a full SP with the 1/2 fractions
				spellPoints.wholeDice++;
				spellPoints.fractionDie = 0;
			}	// if
		}	// if

		/***
		*		After this you need to determine what spells the character knows
		*	Leaving that for a later time, when I have magic spells working.
		***/
	}   // newMage()

	/***
    *       This is the base creator that we need to use to access the public
    *   methods in this class.
    *       
    *       This will create a random characterthat is within a level range.
    ***/
	public Mage(uint minLevel, uint maxLevel)
	{
		/***
		*      Later on this should probably be a reverse progression up to 20,
		*  so lower levels are more common.
		***/
		level = dice.RollDice(1, (maxLevel - minLevel) + 1) + minLevel;
		NewMage();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Mage(uint minLevel, uint maxLevel)

	/***
    *       This is the base creator that we need to use to access the public
    *   methods in this class.
    *       
    *       This will create a random character within a range of levels.
    ***/
	public Mage()
	{
		/***
        *       Later on this should probably be a reverse progression up to 20,
        *   so lower levels are more common.
        ***/
		level = dice.RollDice(1, 20);
		NewMage();
		AdventureGame.Instance.StoryText(GetCharacterInfo());
	}   // Mage()

	/***
	*		This will be written out later.  It will handle casting magic spells for the Mage
	*	character type.  For now just testing using an Interface.
	***/
	public override void CastMagicSpell()
	{
		;
	}   // CastSpell()
}   // class Mage