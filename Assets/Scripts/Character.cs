using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 *      This class contains items that are common to all chacacters.  Specific
 *  character classes are derived from this base class.
***/

public class Character : MonoBehaviour
{
	public const uint minStat = 9;  // This is the lowest a stat can be adjusted  
	public const uint maxLevel = 30;// This is the max level we allow for characters
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
     *      This is the base creator that we need to use to access the public methods in this class.
    ***/
	public Character()
	{   //Instance creator

	}   // Characteristics()
}	// class Character
