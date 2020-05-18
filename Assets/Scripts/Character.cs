using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
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
	public HitDice hits;		// Number of hits the character can take, with wholeDice being whole
								// hits and fractionalDice being any half hits (0 for none, 5 for 1/2)
	[SerializeField]
	public HitDice hitsTaken;   // Set to the hits the character has taken in damage (like hits)
	[SerializeField]
	public double deathMargin;	// Set to the amount of damage over hits the character has before dying
	[SerializeField]
	public uint bonusHits;      // Bonus hits per die * 10 (value of 0, 5, or 10)
	[SerializeField]
	public HitDice[] hitDice = new HitDice[maxLevel + 1];   // Number of hit dice for each level
	[SerializeField]
	public uint[] hitDieRolls = new uint[maxLevel + 1];     // Array of hit die rolls
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
	public uint weight;         // Weight is based on size and gender
	[SerializeField]
	public Gender gender;		// this is set to either male or female
	[SerializeField]
	public uint exp;            // Experience points for this character
	[SerializeField]
	public uint expBonus;       // Experience bonus percent (0, 5, 10, or 15)
	[SerializeField]
	public int attackBonus;		// Attack level bonus
	[SerializeField]
	public int defenseBonus;    // Defense level bonus
	[SerializeField]
	public int effectiveAgility;// This is the effective Agility adjusted by movement speed
	[SerializeField]
	public int aimBonus;        // Archery/Spell casting accuracy bonus
	[SerializeField]
	public HitDice damageBonus;	// For this wholeDice is the multipler and fractionalDie is the add
	[SerializeField]
	public ulong gold;          // Amount of gold the character has
	public Dice dice;		    // Used for rolling dice for this character

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

	public enum Gender
	{
		male,
		female
	}	// enum

	/***
     *		This is the base creator that we need to use to access the public methods in this class.
	 *	All basic elements will be filled in and the character-specific class will make sure it
	 *	meets the minimum requirements and do any adjustments possible.
    ***/
	public Character()
	{   //Instance creator
						//  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
		uint[] weights = { 70, 80, 90,100,110,120,130,135,140,145,150,
						//     11, 12, 13, 14, 15, 16, 17, 18, 19, 20
							  155,160,170,180,190,200,220,250,275,300};
		dice = new Dice();
		strength = dice.RollDice(3, 6);
		intel = dice.RollDice(3, 6);
		wisdom = dice.RollDice(3, 6);
		constitution = dice.RollDice(3, 6);
		dex = dice.RollDice(3, 6);
		agility = dice.RollDice(3, 6);
		size = dice.RollDice(3, 6);
		weight = weights[size];     // May need to make a SetWeight() method later...
		if (dice.RollDice(1, 10) >= 6)
		{   // Set as female and adjust size & weight accordingly
			gender = Gender.female;
			weight = weight - 10;

			for (uint i = 0; i < weights.Length; i++)
			{	// Find what the size needs to be adjusted to by the female weight
				if (weights[i] == weight || weights[i + 1] > weight)
				{   // Use this size and exit the for loop
					size = i;
					break;
				}   // if
			}	// for
		}   // if
		else
		{   // Set as male and don't adjust weight
			gender = Gender.male;
		}	// else

		if (constitution < 13)
			bonusHits = 0;
		else if (constitution < 16)
			bonusHits = 5;  // Bonus hits of 1/2 per die
		else
			bonusHits = 10; // Bonus hits of 1 per die (lucky dog!)

		SetAttackBonus(strength, 0);
		SetDefenseBonus(agility, 0, 12);	// Default to average speed for now
		SetAimBonus(dex, 0);				// Default to just dexterity for the aim bonus
	}   // Characteristics()

	/***
	 *		This will set the accuracy (aimming) bonus for the character based on the dexterity
	 *	value that is passed.  The bonus parameter is whatever bonus you get in addition to the
	 *	normal dexterity bonus, which can be from a magic item or ability with an item.
	 *		Note: Items that are +1, like a bow + 1, are a 5% bonus.  So multiple the number by
	 *	5 to determine the bonus amount.
	***/
	public void SetAimBonus(uint newDex, int bonus)
	{
		aimBonus = (int)newDex + bonus;
	}   // SetAimBonus(uint newDex, uint bonus)

	/***
	 *		This will set the defense bonus based on the agility value that is passed.  This is
	 *	useful when the agility of a character has changed for some reason.  The bonus parameter
	 *	is whatever bonus you get in addition to the normal dexterity bonus, which can be from a
	 *	magic item or ability with an item.
	***/
	public void SetDefenseBonus(uint newAgility, int bonus, uint speed)
	{
		int adjustedAgility = (int)newAgility;

		if (speed <= 6)
			adjustedAgility -= 4;   // Adjust effective agility down by 4
		else if (speed <= 9)
			adjustedAgility -= 2;   // Adjust effective agility down by 2
		else if (speed < 15)
			adjustedAgility -= 0;   // Speeds from 10 to 14 do not adjust agaility
		else if (speed < 18)
			adjustedAgility += 2;   // Adjust effective agility up by 2
		else
			adjustedAgility += 4;   // Adjust effective agility up by 4

		effectiveAgility = adjustedAgility;	// Save this so it can be used for saving throws
		if (adjustedAgility >= 16)
			defenseBonus = bonus + 1;
		else if (adjustedAgility <= 5)
			defenseBonus = bonus - 1;
	}   // SetDefenseBonus(uint newAgility, uint bonus)

	/***
	 *		This will set both the attackBonus and damageBonus values for the character based on
	 *	the passed strength value.  Useful for temporary strength increases, but also for when the
	 *	temporary strength change goes back to normal strength.  The bonus parameter is whatever
	 *	bonus you get in addition to the normal dexterity bonus, which can be from a magic item or
	 *	ability with an item.
	***/
	public void SetAttackBonus(uint newStrength, int bonus)
	{
		
	}   // SetAttackBonus(uint newStrength, uint bonus)

	/***
	 *		The actualHitDice value is what is considered to be hit dice for defense and
	 *	attack levels.  It also is sometimes used for various saving throws.  You take
	 *	the whole hit dice and if there is a 1/2 or 2/3 die you round up.
	***/
	public int ActualHitDice()
	{
		if (hitDice[level].fractionDie == 5 || hitDice[level].fractionDie == 2)
			 return hitDice[level].wholeDice + 1;	// 1/2 or 2/3 adds a die
		else
			return hitDice[level].wholeDice;		// 1/3 doesn't add to hit die count
	}	// ActualHitDice

	/***
	 *		Return the basic stats about this this character.  The calling function can add
	 *	any stats that are character class specific.
	 *		Note: The string does not end with a "\n" in case nothing else is added, but if
	 *	more will be added it should start with a "\n" to start on a new line.
	***/
	public string GetCharacterInfo()
	{
		AdventureGame game = AdventureGame.Instance;
		string stats;

		game.HeadingText(charClassName + " CharType = " + charClass.ToString());
		stats = "Level:        " + level + "\n" +
				"Strength:     " + strength + "\n" +
				"Intelligence: " + intel + "\n" +
				"Wisdom:       " + wisdom + "\n" +
				"Constitution: " + constitution + "\n" +
				"Dexterity:    " + dex + "\n" +
				"Agility:      " + agility + "\n" +
				"Size:         " + size + "\n" +
				"Hits:         " + hits.wholeDice + "." + hits.fractionDie +
								 "Death margin: " + deathMargin +
								 " Hits bonus: " + (((float)bonusHits) / 10.0f) + "/die" +
								 " Hit Dice: " + ActualHitDice() + "\n" +
				"Experience:   " + exp + " (Experience bonus: " + expBonus + "%\n" +
				"Gold:         " + gold +
				"";     // Leave this as the last line so it is easy to add more info above it
		return stats;
	}   // GetCharacterInfo()

	/***
	 *		This method will return the number of hits at the level passed as an argument by
	 *	checking the rolled hits and adding any bonus hits.  It returns a HitDice object so
	 *	the case of a half hit can be shown by setting the fractionalHit value to 5.  You
	 *	would display it as wholeDice + "." fractionalDice.  Fractional hits happen because
	 *	a character has a constitution that give 1/2 hit per die and they are on an odd
	 *	number ofwhole hit dice.  Being on a 1/2 or 2/3 die does not get the constitution
	 *	bonus added to the hits, only whole dice get the bonus.
	 *		In the character class the wholeDice value represents hits and the fractionDice
	 *	value represents halfHits for the character.
	***/
	public HitDice HitsAtLevel(uint lvl)
	{
		HitDice actualHits = new HitDice();
		int bonusAtLevel = hitDice[lvl].wholeDice * (int)bonusHits;

		actualHits.wholeDice = 0;

		for (uint i = 1; i <= lvl; i++)
		{   // Total up all the hits to this level based on what was rolled
			actualHits.wholeDice += hitDice[lvl].wholeDice;
		}   // for

		actualHits.wholeDice += bonusAtLevel / 10;  // Add only the full point hits
		actualHits.fractionDie = bonusAtLevel % 5;  // Set to 5 if there is a half hit bonus
		return actualHits;
	}   // HitsAtLevel(uint lvl)

	/***
	 *		This will roll any hit dice that have not been rolled for the current level.
	 *	If a character has been drained they get back the hits they had previously rolled,
	 *	but if this is a new level (or a new character) it will fill in any levels where
	 *	there is a zero value
	 *		We have to deal with the fea cases where you can go up multiple hit dice for
	 *	a single level  It also has to handle the few cases where you go up a half die and
	 *	a full die in one level.  In some cases you are going up from a fractional die to
	 *	a full die, which will either be from 1/2 or 2/3.  The more normal cases are either
	 *	a full die or a fractional die.
	 *		For a fractional you consider a 1/2 die to be from 1-3 and for a 1/3 die to be
	 *	from 1-2.  If a half
	 *		When the method has returned the hitDieRolls[] array will be correct for the
	 *	characters current level.  In the case of a character that might have been drained
	 *	life levels, previous hits at lost levels will not be re-rolled, but will be
	 *	retained.  Regaining those levels will get those hits back as they were originally.
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
					newHits += dice.RollDice((uint)diceToRoll.wholeDice, 6);
				}   // if

				if (diceToRoll.fractionDie != 0)
				{   // We have a fractional die to roll, so this is either a 2 or 3 for 1/3 or 1/2 die
					newHits += dice.RollDice(1, (uint)diceToRoll.fractionDie);
				}   // if

				hitDieRolls[i] = hitDieRolls[i - 1] + newHits;
			}   // if
		}   // for

		hits = HitsAtLevel(level);
		deathMargin = ( ((double)hits.wholeDice + ((double)hits.fractionDie) / 10) ) *
					  ((double)constitution * 0.03d);	// Margin is hits * constitution * 0.03
}   // RollHits()

	/***
	 *		This method is called by the specific character classes with arrays that say how
	 *	many hit dice this character gets for each level.  Be sure the arrays have maxLevel + 1
	 *	elements in each of them.
	 *		Note: You must have level set for this character or else no hits will be rolled.
	***/
	public void SetHitDice(int[] wholeDice, int[] fractionalDie)
	{
		for (uint i = 0; i <= maxLevel; i++)
		{   // Fill in the hitDice numbers for this character class
			hitDice[level].wholeDice = wholeDice[level];
			hitDice[level].fractionDie = fractionalDie[level];
		}   // for

		RollHits(); // Roll the hits once we know the hit dice.  The level should already be set
	}   // SetHitDice(uint[] wholeDice, uint[] fractionalDice)

	/***
	 *		This method is called when a character is first created to assign some amount of
	 *	gold to them.  The amount is going to be based on the level and the character type,
	 *	so be sure those are set before this is called.
	 *		I just made up these multiples I use, but will have to see how they work out.  It
	 *	is based on the fact that as you go up in levels you tend to find better treasures,
	 *	so you should have more money.  So each set of levels you have a chance at more gold
	 *	for each level and you build and what you earned in the previous set of levels.
	***/
	public void SetGoldAtLevel()
	{
		gold = dice.RollDice(3, 6) * 10;
		if (level <= 5)
			gold += dice.RollDice(level - 1, 5000);
		else
		{   // Level 2 or higher
			gold += dice.RollDice(4, 5000);     // For levels 2-5

			if (level <= 10)
				gold += dice.RollDice(level - 5, 10000);
			else
			{   // level 11 of higher
				gold += dice.RollDice(5, 10000);    // For levels 6-10

				if (level <= 15)
					gold += dice.RollDice(level - 10, 25000);
				else
				{   // level 16 or higher
					gold += dice.RollDice(5, 10000);    // For levels 11-15

					if (level <= 20)
						gold = dice.RollDice(level - 15, 40000);
					else
					{   // Level 21 or higher
						gold = dice.RollDice(5, 50000);     // For levels 16-20

						if (level > 20)
							gold = dice.RollDice(level - 20, 50000);
					}   // if
				}	// if
			}	// if
		}	// if

		switch (charClass)
		{
			case CharType.mage:             // Mage class character
				gold = gold - (gold / 10);  // Mages need to pay to research spells
				break;
			case CharType.fighter:          // Fighter class character
				break;
			case CharType.cleric:           // Cleric class character
				gold = gold - (gold / 10);  // Clerics always give 10% tithe to the church
				break;
			case CharType.theif:            // Theif class character
				break;
			case CharType.dwarf:            // Dwarf class character
				break;
			case CharType.elf:              // Elf class character
				gold = gold - (gold / 20);  // Elves also pay to research spells, but get less spells
				break;
			default:
				break;
		}   // switch
	}   // SetGoldAtLevel()
}   // class Character