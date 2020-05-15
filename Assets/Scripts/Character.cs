using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 *      This class contains items that are common to all chacacters.  Specific
 *  character classes are derived from this base class.
***/

public class Character
{
	public const uint minStat = 9;  // This is the lowest a stat can be adjusted  
	public const uint maxLevel = 30;// This is the max level we allow for characters
	[SerializeField]
	public string charClassName;// This is the character name for this character (Mage, Fighter, etc.)
	[SerializeField]
	public CharType charClass;  // This is the character value for this character (Mage, Fighter, etc.)
	[SerializeField]
	public uint level;          // Level of the character.
	[SerializeField]
	public uint hits;           // Whole number of hits the character can take
	[SerializeField]
	public uint halfHits;       // Set to 5 totoal hits has a franction of 1/2
	[SerializeField]
	public uint bonusHits;      // Bonus hits per die * 10 (value of 0, 5, or 10)
	[SerializeField]
	public HitDice[] hitDice = new HitDice[maxLevel + 1];   // Number of hit dice for each level
	[SerializeField]
	public uint[] hitDieRolls = new uint[maxLevel];   // Array of hit die rolls
	[SerializeField]
	public uint strength;       // Strength points
	[SerializeField]
	public uint intel;          // Intelligence (IQ) points
	[SerializeField]
	public uint wisdom;         // Wisdom points
	[SerializeField]
	public uint constitution;   // Constitution points
	[SerializeField]
	public uint dex;            // Dexterity points
	[SerializeField]
	public uint agility;        // Agility points
	[SerializeField]
	public uint size;           // Size points
	[SerializeField]
	HitDice[] hitDicePerLevel = new HitDice[maxLevel + 1];  // Levels 1 to maxLevel
	[SerializeField]
	public uint exp;            // Experience points for this character
	[SerializeField]
	public uint expBonus;       // Experience bonus percent (0, 5, 10, or 15)
	[SerializeField]
	public ulong gold;          // Amount of gold the character has
	Dice dice = new Dice();     // Used for rolling dice for this character

	/***
	 *		The CharType values are provided to make it easy to have something that works
	 *	in a switch statement in the code.  The charClassName 
	***/
	public enum CharType
	{
		mage,                   // Mage class character
		fighter,                // Fighter class character
		cleric,                 // Cleric class character
		theif,                  // Theif class character
		dwarf,                  // Dwarf class character
		elf                     // Elf class character
	};  // CharType
	/***
     *		This is the base creator that we need to use to access the public methods in this class.
	 *	All basic elements will be filled in and the character-specific class will make sure it
	 *	meets the minimum requirements and do any adjustments possible.
    ***/
	public Character()
	{   //Instance creator
		strength = dice.RollDice(3, 6);
		intel = dice.RollDice(3, 6);
		wisdom = dice.RollDice(3, 6);
		constitution = dice.RollDice(3, 6);
		dex = dice.RollDice(3, 6);
		agility = dice.RollDice(3, 6);
		size = dice.RollDice(3, 6);

		if (constitution < 13)
			bonusHits = 0;
		else if (constitution < 16)
			bonusHits = 5;  // Bonus hits of 1/2 per die
		else
			bonusHits = 10;	// Bonus hits of 1 per die (lucky dog!)
	}   // Characteristics()

	/***
	 *		Return the basic stats about this this character.  The calling function can add
	 *	any stats that are character class specific.
	 *		Note: The string does not end with a "\n" in case nothing else is added, but if
	 *	more will be added it should start with a "\n" to start on a new line.
	***/
	public string GetCharacterInfo()
	{
		AdventureGame game = AdventureGame.Instance;
		string stats, actualHitDice;

		if (hitDice[level].fractionDie == 5 || hitDice[level].fractionDie == 2)
			actualHitDice = (hitDice[level].wholeDice + 1).ToString(); // 1/2 or 2/3 adds a die
		else
			actualHitDice = hitDice[level].wholeDice.ToString();    // 1/3 doesn't add to hit die count

		game.HeadingText(charClassName + " CharType = " + charClass.ToString());
		stats = "Level:        " + level + "\n" +
				"Strength:     " + strength + "\n" +
				"Intelligence: " + intel + "\n" +
				"Wisdom:       " + wisdom + "\n" +
				"Constitution: " + constitution + "\n" +
				"Dexterity:    " + dex + "\n" +
				"Agility:      " + agility + "\n" +
				"Size:         " + size + "\n" +
				"Hits:         " + hits + "." + halfHits.ToString() +
								 " (Hits bonus: 0." + bonusHits + "/die) Hit Dice: " +
								 actualHitDice + "\n" +
				"Experience:   " + exp + " (Experience bonus: " + expBonus + "%\n" +
				"Gold:         " + gold +
				"";     // Leave this as the last line so it is easy to add more info above it
		return stats;
	}   // GetCharacterInfo()

	/***
	 *		This method will return the number of hits at the level passed as an argument by
	 *	checking the rolled hits and adding any bonus hits.
	***/
	public HitDice HitsAtLevel(uint lvl)
	{
		HitDice actualHits = new HitDice();
		uint bonusAtLevel = hitDice[lvl].wholeDice * bonusHits;

		actualHits.wholeDice = 0;

		for (uint i = 1; i <= lvl; i++)
		{   // Total up all the hits to this level based on what was rolled
			actualHits.wholeDice += hitDice[lvl].wholeDice;
		}   // for

		actualHits.wholeDice += bonusAtLevel / 10;  // Add only the full hits
		actualHits.fractionDie = bonusAtLevel % 5;  // Set to 5 if there is a half hit
		return actualHits;
	}   // HitsAtLevel(uint lvl)

	/***
	 *		This will roll any hit dice that have not been rolled for the current level.
	 *	If a character has been drained they get back the hits they had previously rolled,
	 *	but if this is a new level (or a new character) it will fill in any levels where
	 *	there is a zero value
	***/
	public void RollHits()
	{
		AdventureGame game = AdventureGame.Instance;
		HitDice totalHits;

		for (uint i = 1; i <= level; i++)
		{   // Note that level 0 is always left at 0
			if (hitDieRolls[i] == 0)
			{   // We need to roll hits for this level, so get local copies last level and this one
				HitDice lastLevel = hitDice[level - 1];
				HitDice thisLevel = hitDice[level];
				HitDice diceToRoll = new HitDice();
				uint newHits = 0;

				diceToRoll.wholeDice = thisLevel.wholeDice - lastLevel.wholeDice;
				diceToRoll.fractionDie = 0;
				if (lastLevel.fractionDie != 0 && lastLevel.fractionDie != thisLevel.fractionDie)
				{   // We are going up from a fractional die to a different fraction or whole die
					if (thisLevel.fractionDie == 0)
					{   // We are going from a fractional die to a whole die, so figure the fraction
						if (lastLevel.fractionDie == 5)
							diceToRoll.fractionDie = 3; // We need to roll a 3-sided die for 1/2 die
						else
							diceToRoll.fractionDie = 2; // We need to roll a 2-sided die for 1/3 die
					}   // if
					else if (thisLevel.fractionDie == 5)
						diceToRoll.fractionDie = 3;     // We need to roll a 3-sided die for 1/2 die
					else
						diceToRoll.fractionDie = 2;  // We need to roll a 2-sided die for 1/3 die

					if (diceToRoll.wholeDice != 0)
						diceToRoll.wholeDice--; // We are rolling up from a factional to a whole die
				}   // if

				if (diceToRoll.wholeDice != 0)
				{   // We have a change in whole dice since the previous level (may be more than 1)
					newHits += dice.RollDice(diceToRoll.wholeDice, 6);
				}   // if

				if (diceToRoll.fractionDie != 0)
				{   // We have a fractional die to roll, so this is either a 2 or 3 for 1/3 or 1/2 die
					newHits += dice.RollDice(1, diceToRoll.fractionDie);
				}   // if

				hitDieRolls[i] = hitDieRolls[i - 1] + newHits;
			}   // if
		}   // for

		totalHits = HitsAtLevel(level);
		hits = totalHits.wholeDice;
		halfHits = totalHits.fractionDie;
	}   // RollHits()

	/***
	 *		This method is called by the specific character classes with arrays that say how
	 *	many hit dice this character gets for each level.  Be sure the arrays have maxLevel + 1
	 *	elements in each of them.
	 *		Note: You must have level set for this character or else no hits will be rolled.
	***/
	public void SetHitDice(uint[] wholeDice, uint[] fractionalDie)
	{
		for (uint i = 0; i <= maxLevel; i++)
		{   // Fill in the hitDice numbers for this character class
			hitDice[level].wholeDice = wholeDice[level];
			hitDice[level].fractionDie = fractionalDie[level];
		}   // for

		RollHits(); // Roll the hits once we know the hit dice.  The level should already be set
	}   // SetHitDice(uint[] wholeDice, uint[] fractionalDice)
}   // class Character
