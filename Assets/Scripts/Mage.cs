using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{
	[SerializeField]
	public uint spellPoints;// Whole number of spell points the character has
	[SerializeField]
	public uint halfSP;     // Set to 5 totoal spell points has a franction of 1/2
	[SerializeField]
	public uint bonusSP;    // Set to bonus SP per die * 10 (value of 0, 5, or 10)
	[SerializeField]
	MagicSpells[,] spells = new MagicSpells[maxLevel + 1, 8]; // Need to define this class
	[SerializeField]
	uint magicClass;
	HitDice[] hitDicePerLevel = new HitDice[maxLevel + 1];  // Levels 1 to maxLevel
	Dice dice = new Dice(); // Always have a Dice object for rolling dice
	static readonly uint[] wholeDice =
		new uint[] {0, 1, 1, 2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 9, 10, 10, 11, 11,
					12,12,13,13,14,14,14,15,15,15,16,16,16,17,17};
	static readonly uint[] fractionalDice =
		new uint[] { 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 0, 5, 0, 5, 0, 5,
					 0, 5, 0, 5, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1};
	/***
	 *		This method calls GetCharacterInfo() from the parent Character class and adds
	 *	items specific to the Mage class.
	***/
	public string GetMageInfo()
	{
		string fractionSP;

		if (bonusSP == 5)
			fractionSP = "+0.5";
		else if (bonusSP == 10)
			fractionSP = "+1";
		else
			fractionSP = "+0;";

		return GetCharacterInfo() + "\n" +
			   "Spell points: " + spellPoints + "." + halfSP.ToString() + fractionSP + "SP/die\n" +
			   "Magic class:: " + magicClass + "\n" +
			   "";
	}	// GetMageInfo()

	/***
     *      Adjust IQ up to a goal by lowering Wisdom.  This is generally preferred
     *  over reducing Strength, because Strength affects carrying capacity and the
     *  characters fighting ability.
    ***/
	void RaiseIQfromWisdom(uint goal)
	{
		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < minStat + 3)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				intel++;
				wisdom -= 3;
			}   // else
		}   // while
	}   // raiseIQfromWisdom()

	/***
     *      Adjust IQ up to Min by lowering Strength.
    ***/
	void RaiseIQfromStrength(uint goal)
	{
		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Strength
			if (strength < minStat + 2)
				break;  // We can't adjust Strength any lower
			else
			{   // Adjust IQ up one point and Strength down 2 points
				intel++;
				strength -= 2;
			}   // else
		}   // while
	}   // raiseIQfromStrength()

	/***
     *      This will create a random character adjusted to be the best Mage
     *  possible.
    ***/
	public void NewMage()
	{
		charClassName = "Mage";
		charClass = CharType.mage;

		/***
         *      Roll all characteristics that are not required for a Mage first.
        ***/
		strength = dice.RollDice(3, 6);
		wisdom = dice.RollDice(3, 6);
		constitution = dice.RollDice(3, 6);
		dex = dice.RollDice(3, 6);
		agility = dice.RollDice(3, 6);
		size = dice.RollDice(3, 6);

		/***
         *      Now we need to generate the Intelligence (IQ) characteristic, which
         *  has a minimum requirement for a Mage of 9.  A Mage can adjust the IQ
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

		while ((intel = dice.RollDice(3, 6)) < 9)
			;   // Continue generating the IQ for this Mage until it is at least 9

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
			expBonus = 0;   // No experience bonus
		else if (intel < 15)
			expBonus = 5;   // 5% experience bonus
		else if (intel < 19)
			expBonus = 10;  // 10% experience bonus
		else
			expBonus = 15;  // 15% experience bonus

		magicClass = (uint)Random.Range(1.0f, 6.0f);    // Determine a magic class
		hitDice = new HitDice();
		hitDice.wholeDice = wholeDice[level];
		hitDice.fractionDie = fractionalDice[level];

		/***
		 *		After this you need to determine what spells the character knows
		 *	Leaving that for a later time, when I have magic spells working.
		***/
	}   // newMage()

	/***
     *      This is the base creator that we need to use to access the public
     *  methods in this class.
     *      
     *      This will create a random characterthat is within a level range.
    ***/
	public Mage(uint minLevel, uint maxLevel)
	{
		level = dice.RollDice(1, (maxLevel - minLevel) + 1) + minLevel;
		NewMage();
		AdventureGame.Instance.StoryText(GetMageInfo());
	}   // Mage()

	/***
     *      This is the base creator that we need to use to access the public
     *  methods in this class.
     *      
     *      This will create a random character within a range of levels.
    ***/
	public Mage()
	{
		/***
         *      Later on this should probably be a reverse progression up to 20,
         *  so lower levels are more common.
        ***/
		level = dice.RollDice(1, 20);
		NewMage();
		AdventureGame.Instance.StoryText(GetMageInfo());
	}   // Mage()
}	// class Mage
