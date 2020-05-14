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
	public CharType charClass;	// This is the character value for this character (Mage, Fighter, etc.)
	[SerializeField]
	public uint level;          // Level of the character.
	[SerializeField]
	public uint hits;           // Whole number of hits the character can take
	[SerializeField]
	public uint halfHits;       // Set to 5 totoal hits has a franction of 1/2
	[SerializeField]
	public uint bonusHits;      // Bonus hits per die * 10 (value of 0, 5, or 10)
	[SerializeField]
	public HitDice hitDice;     // Number of hit dice for this level
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
	public uint exp;            // Experience points for this character
	[SerializeField]
	public uint expBonus;       // Experience bonus percent (0, 5, 10, or 15)
	[SerializeField]
	public ulong gold;          // Amount of gold the character has

	/***
	 *		The CharType values are provided to make it easy to have something that works
	 *	in a switch statement in the code.  The charClassName 
	***/
	public enum CharType
	{
		mage,					// Mage class character
		fighter,				// Fighter class character
		cleric,					// Cleric class character
		theif,					// Theif class character
		dwarf,					// Dwarf class character
		elf						// Elf class character
	};	// CharType
	/***
     *      This is the base creator that we need to use to access the public methods in this class.
    ***/
	public Character()
	{   //Instance creator

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

		if (hitDice.fractionDie == 5 || hitDice.fractionDie == 2)
			actualHitDice = (hitDice.wholeDice + 1).ToString(); // 1/2 or 2/3 adds a die
		else
			actualHitDice = hitDice.wholeDice.ToString();       // 1/3 does not add to hit die count

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
}   // class Character
