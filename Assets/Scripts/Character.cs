﻿using System;
using UnityEngine;

/***
*      The idea for this interface is to create an interface that will be able to
*  handle everything needed for casting magic.  Any character can cast magic, but
*  they might be limited to an item if they are not natively a magic user.  So it
*  is provided as an interface for the base Character class.
*      Mathods will be provided that can handle casting of spells, which includes
*  aiming, speed of the casting, and effects on both the caster and the target(s).
***/

public interface IMagical
{
	void CastMagicSpell();
}   // interface IMagic

/***
*      The idea for this interface is to create an interface that will be able to
*  handle everything needed for casting magic.  Any character can cast magic, but
*  they might be limited to an item if they are not natively a magic user.  So it
*  is provided as an interface for the base Character class.
*      Mathods will be provided that can handle casting of spells, which includes
*  aiming, speed of the casting, and effects on both the caster and the target(s).
***/

public interface IFight
{
	void TakeBlow();
}   // interface IMagic

/***
*      The idea for this interface is to create an interface that will be able to
*  handle everything needed for casting magic.  Any character can cast magic, but
*  they might be limited to an item if they are not natively a magic user.  So it
*  is provided as an interface for the base Character class.
*      Mathods will be provided that can handle casting of spells, which includes
*  aiming, speed of the casting, and effects on both the caster and the target(s).
***/

public interface IClerical
{
	void CastClericalSpell();
}   // interface IMagic

/***
*      The idea for this interface is to create an interface that will be able to
*  handle everything needed for casting magic.  Any character can cast magic, but
*  they might be limited to an item if they are not natively a magic user.  So it
*  is provided as an interface for the base Character class.
*      Mathods will be provided that can handle casting of spells, which includes
*  aiming, speed of the casting, and effects on both the caster and the target(s).
***/

public interface IThievish
{
	void DoThievish();
}   // interface IMagic

/***
*      This class contains items that are common to all chacacters.  Specific
*  character classes are derived from this base class.
***/

public class Character : IMagical, IFight, IClerical, IThievish
{
	public const int minStat = 9;  // This is the lowest a stat can be adjusted  
	public const int maxLevel = 30;// This is the max level we allow for characters
	public const int maxMagicSpellLevel = 8;
	[SerializeField]
	public string charClassName;// This is the character name for this character (Mage, Fighter, etc.)
	[SerializeField]
	public CharType charClass;  // This is the character value for this character (Mage, Fighter, etc.)
	[SerializeField]
	public int level;          // Level of the character.
	[SerializeField]
	public HitDice hits;        // Number of hits the character can take, with whole being whole
								// hits and fractionalalDice being any half hits (0 for none, 5 for 1/2)
	[SerializeField]
	public HitDice hitsTaken;   // Set to the hits the character has taken in damage (like hits)
	[SerializeField]
	public double deathMargin;  // Set to the amount of damage over hits the character has before dying
	[SerializeField]
	public int bonusHits;      // Bonus hits per die* 10 (value of -10, -5, 0, 5, or 10)
	[SerializeField]
	public EasyFrac[] hitDice = new EasyFrac[maxLevel + 1];   // Number of hit dice for each level
	[SerializeField]
	public HitDice[] hitDieRolls = new HitDice[maxLevel + 1];   // Array of hit die rolls
	[SerializeField]
	public int strength;        // Strength points
	[SerializeField]
	public int intel;           // Intelligence (IQ) points
	[SerializeField]
	public int wisdom;          // Wisdom points
	[SerializeField]
	public int constitution;    // Constitution points
	[SerializeField]
	public int dex;             // Dexterity points
	[SerializeField]
	public int agility;         // Agility points
	[SerializeField]
	public int size;            // Size points
	[SerializeField]
	public int weight;          // Weight is based on size and gender
	[SerializeField]
	public Gender gender;       // this is set to either male or female
	[SerializeField]
	public int exp;             // Experience points for this character
	[SerializeField]
	public int expBonus;        // Experience bonus percent (0, 5, 10, or 15)
	[SerializeField]
	public int attackBonus;     // Attack level bonus
	[SerializeField]
	public int defenseBonus;    // Defense level bonus
	[SerializeField]
	public int effectiveAgility;// This is the effective Agility adjusted by movement speed (can be < 0)
	[SerializeField]
	public int aimBonus;        // Archery/Spell casting accuracy bonus
	[SerializeField]
	public int damageMultiplier;// This is the multiplier for how many damage dice to roll for a weapon
	[SerializeField]
	public EasyFrac damageBonus; // This is the amount to add for each original die of damage of a weapon
	[SerializeField]
	public int[] carryCapacity; // Carry capacity for 3, 6, 9, 12, 15, 18, 21 inches per phase
	[SerializeField]
	public int carriedWeight;   // This is the amount of weight the character is currently carrying
	[SerializeField]
	public int speed;			// This is the current speed the character is able to make per turn
	[SerializeField]
	public long gold;           // Amount of gold the character has
	[SerializeField]
	public HitDice spellPoints; // Whole number of spell points the character has
	[SerializeField]
	public double spellPointMargin; // Spellpoint margin for emergency use
	[SerializeField]
	public int bonusSP;         // Set to bonus SP per die* 10 (value of 0, 5, or 10)
	[SerializeField]
	public MagicSpells[,,] spells;  // All indexes will contain arrays of MagicSpells
	[SerializeField]
	public int magicClass;
	public Dice dice = new Dice();  // Used for rolling dice for this character
	public Fraction fraction;       // Used to working with fractions
	static readonly int[] weights =
		{							// 0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
									  70, 80, 90,100,110,120,130,135,140,145,150,
									//11, 12, 13, 14, 15, 16, 17, 18, 19, 20
									 155,160,170,180,190,200,220,250,275,300
		};  // weights[]
	static readonly double[] capacityMultiplier =
		{							//   0,   1,   2,   3,   4,   5,   6,   7,   8,   9,  10,
									   0.0, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.6, 1.8, 2.0,
										//	 11,  12,  13,  14,  15,  16,  17,  18,  19,  20,
											2.0, 2.2, 2.4, 2.6, 2.7, 2.8, 2.9, 3.0, 3.1, 3.2,
										//	 21,  22,  23,  24,  25,  26,  27,  28,  29,  30,
											3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 4.0, 4.1, 4.2,
										//	 31,  32,  33,  34,  35,  36,  37,  38,  39,  40,
											4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 4.9, 5.0, 5.1, 5.2,
										//	 41,  42,  43,  44,  45,  46,  47,  48,  49,  50,
											5.3, 5.4, 5.5, 5.6, 5.7, 5.8, 5.9, 6.0, 6.1, 6.2,
										//	 51,  52,  53,  54,  55,  56,  57,  58,  59,  60,
											6.3, 6.4, 6.5, 6.6, 6.7, 6.8, 6.9, 7.0, 7.1, 7.2,
										//	 61,  62,  63,  64,  65,  66,  67,  68,  69,  70,
											7.3, 7.4, 7.5, 7.6, 7.7, 7.8, 7.9, 5.0, 5.1, 5.2,
										//	 71,  72,  73,  74,  75,  76,  77,  78,  79,  80,
											8.3, 8.4, 8.5, 8.6, 8.7, 8.8, 8.9, 9.0, 9.1, 9.2,
										//	 81,  82,  83,  84,  85,  86,  87,  88,  89,  90,
											9.3, 9.4, 9.5, 9.6, 9.7, 9.8, 9.9,10.0,10.1,10.2,
										//	 91,  92,  93,  94,  95,  96,  97,  98,  99, 100,
										   10.3,10.4,10.5,10.6,10.7,10.8,10.9,11.0,11.1,11.2,
		};  // capacityMultiplier[]
	static readonly int[] attackLevelBonuses =
		{							//   0,  1,  2,  3,  4,  5,  6,  7,  8,  9,
										-1, -1, -1, -1, -1, -1,  0,  0,  0,  0,
									//  10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
										 0,  0,  0,  0,  0,  0,  1,  1,  1,  1,
									//  20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
										 1,  1,  1,  1,  1,  1,  2,  2,  2,  2,
									//  30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
										 2,  2,  2,  2,  2,  2,  2,  2,  2,  2,
									//  40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
										 2,  3,  3,  3,  3,  3,  3,  3,  3,  3,
									//  50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
										 3,  3,  3,  3,  3,  3,  3,  3,  3,  3,
									//  60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
										 3,  4,  4,  4,  4,  4,  4,  4,  4,  4,
									//  70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
										 4,  4,  4,  4,  4,  4,  4,  4,  4,  4,
									//  80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
										 4,  4,  4,  4,  4,  4,  5,  5,  5,  5,
									//  90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
										 5,  5,  5,  5,  5,  5,  5,  5,  5,  5,
									// 100
										 5
		};  // attackLevelBonuses[]
	static readonly int[] damageMultipliers =
			{                       //   0,  1,  2,  3,  4,  5,  6,  7,  8,  9,
										 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,
									//  10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
										 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,
									//  20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
										 1,  1,  1,  1,  1,  1,  1,  1,  2,  2,
									//  30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
										 2,  2,  2,  2,  2,  2,  3,  3,  3,  3,
									//  40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
										 3,  3,  3,  4,  4,  4,  4,  4,  4,  4,
									//  50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
										 5,  5,  5,  5,  5,  5,  5,  6,  6,  6,
									//  60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
										 6,  6,  6,  6,  7,  7,  7,  7,  7,  7,
									//  70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
										 7,  8,  8,  8,  8,  8,  8,  8,  9,  9,
									//  80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
										 9,  9,  9,  9,  9, 10, 10, 10, 10, 10,
									//  90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
										10, 10, 11, 11, 11, 11, 11, 11, 11, 12,
									// 100
										12
		};  // damageMultipliers[]
	static readonly int[] damageBonuses = // Values are 5=0.5, 10=1.0, 15=1.5, 20=2.0, and 25=2.5
			{                       //   0,  1,  2,  3,  4,  5,  6,  7,  8,  9,
									   -10,-10,-10,-10,-10, -5, -5, -5, -5,  0,
									//  10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
										 0,  0,  0,  5,  5,  5,  5,  5, 10, 10,
									//  20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
										10, 10, 15, 15, 15, 20, 20, 25,  0,  0,
									//  30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
										 0,  5, 10, 15, 20, 25,  0,  0,  5, 10,
									//  40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
										15, 20, 25,  0,  0,  5, 10, 15, 20, 25,
									//  50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
										 0,  0,  5, 10, 15, 20, 25,  0,  0,  5,
									//  60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
										10, 15, 20, 25,  0,  0,  5, 10, 15, 20,
									//  70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
										25,  0,  0,  5, 10, 15, 20, 25,  0,  0,
									//  80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
										 5, 10, 15, 20, 25,  0,  0,  5, 10, 15,
									//  90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
										20, 25,  0,  0,  5, 10, 15, 20, 25,  0,
									// 100
										 0
		};  // damageMultipliers[]
	static readonly double[] speedClassDivisors =
									// 0,   3,   6,   9,  12,  15,  18,  21 inches per turn
									{  1,1.01, 2.0, 3.0, 4.0, 6.0, 8.0,12.0};
	static readonly int[] speeds =  {  0,   3,   6,   9,  12,  15,  18,  21};
	/***
		*		The CharType values are provided to make it easy to have something that works
		*	in a switch statement in the code.  The character Class Name as a number is easy to
		*	check in conditionals.
		***/
	public enum CharType
	{
		mage,                   // Mage class character
		fighter,                // Fighter class character
		cleric,                 // Cleric class character
		thief,                  // Thief class character
		dwarf,                  // Dwarf class character
		elf                     // Elf class character
	};  // CharType

	/***
	*		This provides the values used to determine gender of a character (don't want to
	*	use strings or constant numbers).
	***/
	public enum Gender
	{
		male,
		female
	}   // enum

	/***
	*		This method will determine what the character's current speed is based on what
	*	it is currently carrying.  So this should be called any time the weight being carried
	*	or strength changes.  If size ever changes it should be called as well, but that is 
	*	unlikely to happen.
	***/
	public void SetSpeed()
	{
		for (int i = 0; i < speeds.Length; i++)
		{
			if (carriedWeight <= carryCapacity[i])
				speed = speeds[i];	// Last one that is <= is our speed class
		}	// for
	}   // SetSpeed()

	/***
	*		This method is called when Strength is changed.  This may be called any time strength
	*	changes due to a magic spell or item being used, or during character creation when the
	*	strength gets adjusted to improve another characteristic.  If a spell or item is used the
	*	strength will need to be changed again when the magical enhancement has stopped.
	***/
	public void SetStrength(int newStrength)
	{
		double realWeight;
		int adjustedStrength;

		strength = newStrength;
		if (charClass == CharType.elf || charClass == CharType.dwarf)
		{   // These carry as the rolled size, not actual size
			realWeight = (double)weights[size + 2];
			if (gender == Gender.female)
				realWeight -= 10;   // We still have to subtract 10 pounds for being female
		}   // if
		else
			realWeight = weight;    // This is already the correct weight

		for (int i = 0; i < carryCapacity.Length; i++)
		{   // Calculate carrying capacity for speeds from 0 to 21 inches per turn
			carryCapacity[i] = (int)((realWeight * capacityMultiplier[strength])
									  / speedClassDivisors[i]);
		}   // for

		/***
		*		May want to extend the strength bonus tables to be beyond 100.  I have the table
		*	that goes way beyond that.  It is possible to have a character with over 20 strength
		*	and with Super Strength that can exceed 100.  Also useful if you have a monster that
		*	has a particular damage it does to figure out the attack bonus it should have, which
		*	might be higher than 100 strength.
		***/

		if (strength > damageMultipliers.Length - 1)
			adjustedStrength = (int)damageMultipliers.Length - 1;  // Prevent going beyond array end
		else
			adjustedStrength = strength;

		SetSpeed();
		attackBonus = attackLevelBonuses[adjustedStrength];
		damageMultiplier = damageMultipliers[adjustedStrength];
		if (damageBonus == null)
			damageBonus = new EasyFrac();	// Allocate the first time we do this

		damageBonus.SetValue(damageBonuses[adjustedStrength] / 10,	// Will be (+/-) 0, 1, or 2
							 damageBonuses[adjustedStrength] % 10);	// Will be (+/-) 0 or 5
	}   // SetStrength(int newStrength)

	/***
	*		This figures the current SP margin and SP values, using the hits and bonus SP values.
	***/
	public void SetSpellPoints()
	{
		int totalBonusSP = hitDice[level].whole * bonusSP;

		spellPointMargin = deathMargin;
		spellPoints = new HitDice(hits.whole, hits.fractional.fraction);
		spellPoints.whole += totalBonusSP / 10;
		if (bonusSP == 5 && (hitDice[level].whole & 1) == 1)
		{   // For odd whole hit dice this character gets a 1/2 SP
			spellPoints.fractional.fraction += 5;
			if (spellPoints.fractional.fraction == 10)
			{   // We rounded up to a full SP with the 1/2 fractions
				spellPoints.whole++;
				spellPoints.fractional.fraction = 0;
			}   // if
		}   // if
	}   // SetSpellPoints()

	/***
    *		This is the base creator that we need to use to access the public methods in this class.
	*	All basic elements will be filled in and the character-specific class will make sure it
	*	meets the minimum requirements and do any adjustments possible.
    ***/
	public Character()
	{
		fraction = new Fraction();
		carryCapacity = new int[speeds.Length];
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
			weight -= 10;

			for (int i = 0; i < weights.Length; i++)
			{   // Find what the size needs to be adjusted to by the female weight
				if (weights[i] == weight || weights[i + 1] > weight)
				{   // Use this size and exit the for loop
					size = i;
					break;
				}   // if
			}   // for
		}   // if
		else
		{   // Set as male and don't adjust weight & size
			gender = Gender.male;
		}   // else

		SetStrength(dice.RollDice(3, 6));   // Set Attack and Damage bonuses, plus carrying capacities
		SetDefenseBonus(agility, 0, speed);
		SetAimBonus(dex, 0);                // Default to just dexterity for the aim bonus

		for (int i = 0; i <= maxLevel; i++)
			hitDieRolls[i] = new HitDice(); // Pre-allocate the elements of this array now

		if (constitution <= 5)
			bonusHits = -10;    // Bonus hits of -1 per die
		else if (constitution <= 9)
			bonusHits = -5;     // Bonus hits of -1/2 per die
		else if (constitution < 13)
			bonusHits = 0;
		else if (constitution < 16)
			bonusHits = 5;      // Bonus hits of 1/2 per die
		else
			bonusHits = 10;     // Bonus hits of 1 per die (lucky dog!)

		spells = new MagicSpells[maxLevel + 1, maxMagicSpellLevel, 2];  // To be filled in later
	}   // Character()

	/***
    *       Adjust Stringth up to a goal by lowering Wisdom.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   character's fighting ability.  This can only be used by a fighting type character.
    ***/
	public void RaiseStrengthfromWisdom(int goal)
	{
		int newStrength = strength;

		while (newStrength < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < minStat + 3)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				newStrength++;
				wisdom -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseStrengthfromWisdom(int goal)

	/***
    *       Adjust Stringth up to a goal by lowering Wisdom.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   character's fighting ability.  This can only be used by a fighting type character.
    *		It will not let wisdom go below a minimum level.  This is used when a character
    *	needs wisdom, but strength may be more important to reach a specific goal
    ***/
	public void RaiseStrengthfromWisdom(int goal, int min)
	{
		int newStrength = strength;

		while (newStrength < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < min + 3)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				newStrength++;
				wisdom -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseStrengthfromWisdom(int goal, int min)

	/***
    *       Adjust Stringth up to a goal by lowering Wisdom.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   character's fighting ability.  This can only be used by a fighting type character.
    ***/
	public void RaiseStrengthfromIQ(int goal)
	{
		int newStrength = strength;

		while (newStrength < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (intel < minStat + 2)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				newStrength++;
				intel -= 2;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseStrengthfromIQ(int goal)

	/***
    *       Adjust Stringth up to a goal by lowering Wisdom.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   character's fighting ability.  This can only be used by a fighting type character.
    *		It will not let IQ go below a minimum level.  This is used when a character
    *	needs IQ, but strength may be more important to reach a specific goal
    ***/
	public void RaiseStrengthfromIQ(int goal, int min)
	{
		int newStrength = strength;

		while (newStrength < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (intel < min + 2)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				newStrength++;
				intel -= 2;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseStrengthfromIQ(int goal, int min)

	/***
    *       Adjust IQ up to a goal by lowering Wisdom.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   characters fighting ability.  This can only be used by a magic type charcter.
    ***/
	public void RaiseIQfromWisdom(int goal)
	{
		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < minStat + 2)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				intel++;
				wisdom -= 2;
			}   // else
		}   // while
	}   // raiseIQfromWisdom(int goal)

	/***
    *       Adjust IQ up to Min by lowering Strength.  This can only be used by
    *	magic type characters.
    ***/
	public void RaiseIQfromStrength(int goal)
	{
		int newStrength = strength;

		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Strength
			if (newStrength < minStat + 3)
				break;  // We can't adjust Strength any lower
			else
			{   // Adjust IQ up one point and Strength down 2 points
				intel++;
				newStrength -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // raiseIQfromStrength(int goal)

	/***
    *       Adjust IQ up to Min by lowering Strength.  This can only be used by
    *	magic type characters.
    ***/
	public void RaiseIQfromStrength(int goal, int min)
	{
		int newStrength = strength;

		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Strength
			if (newStrength < min + 3)
				break;  // We can't adjust Strength any lower
			else
			{   // Adjust IQ up one point and Strength down 2 points
				intel++;
				newStrength -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // raiseIQfromStrength(int goal)

	/***
    *       Adjust Wisdom up to a goal by lowering IQ.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   characters fighting ability.  This can only be used by a cleric type charcter.
    ***/
	public void RaiseWisdomfromIQ(int goal)
	{
		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < minStat + 2)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				intel++;
				wisdom -= 2;
			}   // else
		}   // while
	}   // RaiseWisdomfromIQ(int goal)

	/***
    *       Adjust Wisdom up to a goal by lowering IQ.  This is generally preferred
    *   over reducing Strength, because Strength affects carrying capacity and the
    *   characters fighting ability.  This can only be used by a cleric type charcter.
    ***/
	public void RaiseWisdomfromIQ(int goal, int min)
	{
		while (intel < goal)
		{   // Get as close as we can to the goal by lowering Wisdom
			if (wisdom < min + 2)
				break;  // We can't adjust Wisdom any lower
			else
			{   // Adjust IQ up one point and Wisdom down 3 points
				intel++;
				wisdom -= 2;
			}   // else
		}   // while
	}   // RaiseWisdomfromIQ(int goal)

	/***
    *       Adjust Wisdom up to a goal by lowering Strength.  Only Clerical characters
    *   can use this
    ***/
	public void RaiseWisdomfromStrength(int goal)
	{
		int newStrength = strength;

		while (wisdom < goal)
		{   // Get as close as we can to the goal by lowering Strength
			if (newStrength < minStat + 3)
				break;  // We can't adjust Strength any lower
			else
			{   // Adjust IQ up one point and Strength down 2 points
				wisdom++;
				newStrength -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseWisdomfromStrength(int goal)

	/***
    *       Adjust Wisdom up to a goal by lowering Strength.  Only Clerical characters
    *   can use this
    ***/
	public void RaiseWisdomfromStrength(int goal, int min)
	{
		int newStrength = strength;

		while (wisdom < goal)
		{   // Get as close as we can to the goal by lowering Strength
			if (newStrength < min + 3)
				break;  // We can't adjust Strength any lower
			else
			{   // Adjust IQ up one point and Strength down 2 points
				wisdom++;
				newStrength -= 3;
			}   // else
		}   // while

		if (strength != newStrength)
			SetStrength(newStrength);
	}   // RaiseWisdomfromStrength(int goal)

	/***
	*		This will set the accuracy (aimming) bonus for the character based on the dexterity
	*	value that is passed.  The bonus parameter is whatever bonus you get in addition to the
	*	normal dexterity bonus, which can be from a magic item or ability with an item.
	*		Note: Items that are +1, like a bow + 1, are a 5% bonus.  So multiple the number by
	*	5 to determine the bonus amount.
	***/
	public void SetAimBonus(int newDex, int bonus)
	{
		aimBonus = (int)newDex + bonus;
	}   // SetAimBonus(int newDex, int bonus)

	/***
	*		This will set the defense bonus based on the agility value that is passed.  This is
	*	useful when the agility of a character has changed for some reason.  The bonus parameter
	*	is whatever bonus you get in addition to the normal dexterity bonus, which can be from a
	*	magic item or ability with an item.
	***/
	public void SetDefenseBonus(int newAgility, int bonus, int speed)
	{
		int adjustedAgility = (int)newAgility;

		if (speed <= 6)
			adjustedAgility -= 4;   // Adjust effective agility down by 4
		else if (speed <= 9)
			adjustedAgility -= 2;   // Adjust effective agility down by 2
		else if (speed <= 15)
			adjustedAgility -= 0;   // Speeds from 10 to 14 do not adjust agaility
		else if (speed <= 18)
			adjustedAgility += 2;   // Adjust effective agility up by 2
		else
			adjustedAgility += 4;   // Adjust effective agility up by 4

		effectiveAgility = adjustedAgility; // Save this so it can be used for saving throws
		if (adjustedAgility >= 16)
			defenseBonus = bonus + 1;
		else if (adjustedAgility <= 5)
			defenseBonus = bonus - 1;
	}   // SetDefenseBonus(int newAgility, int bonus)

	/***
	*		The actualHitDice value is what is considered to be hit dice for defense and
	*	attack levels.  It also is sometimes used for various saving throws.  You take
	*	the whole hit dice and if there is a 1/2 or 2/3 die you round up.
	***/
	public int ActualHitDice()
	{
		if (hitDice[level].fractional.fraction == Fraction.oneHalf
			|| hitDice[level].fractional.fraction == Fraction.twoThirds)
			return hitDice[level].whole + 1;    // 1/2 or 2/3 adds a die
		else
			return hitDice[level].whole;        // 1/3 doesn't add to hit die count
	}   // ActualHitDice

	/***
	*		Return the basic stats about this this character.  The calling function can add
	*	any stats that are character class specific.
	*		Note: The string does not end with a "\n" in case nothing else is added, but if
	*	more will be added it should start with a "\n" to start on a new line.
	***/
	public virtual string GetCharacterInfo()
	{
		AdventureGame game = AdventureGame.Instance;
		string stats;
		stats = "Level:\t\t\t\t" + level + "\n" +
				"Strength:\t\t\t" + strength + "\n" +
				"Intelligence:\t\t" + intel + "\n" +
				"Wisdom:\t\t\t" + wisdom + "\n" +
				"Constitution:\t" + constitution + "\n" +
				"Dexterity:\t\t\t" + dex + "\n" +
				"Agility:\t\t\t\t" + agility + "\n" +
				"Size:\t\t\t\t\t" + size + "\n" +
				"Attack Bonus:\t" + attackBonus + "\n" +
				"Damage:\t\t\tMultiplier=" + damageMultiplier +
								 "\tBonus=" + damageBonus.ToString() + "/die\n" +
				"Hits:\t\t\t\t\t" + hits.whole + "." + hits.fractional.fraction +
								 "\t\tDeath margin: " + deathMargin +
								 "\t\tHits bonus: " + (((float)bonusHits) / 10.0f) + "/die" +
								 "\t\tHit Dice: " + ActualHitDice() + "\n" +
				"Experience:\t\t" + exp + "\t\t\tExperience bonus: " + expBonus + "%\n" +
				"Gold:\t\t\t\t" + gold +
				"";     // Leave this as the last line so it is easy to add more info above it
		return stats;
	}   // GetCharacterInfo()

	/***
	*		This method will return the number of hits at the level passed as an argument by
	*	checking the rolled hits and adding any bonus hits.  It returns a HitDice object so
	*	the case of a half hit can be shown by setting the fractionalHit value to 5.  You
	*	would display it as whole + "." fractionalDice.  Fractional hits happen because
	*	a character has a constitution that give 1/2 hit per die and they are on an odd
	*	number ofwhole hit dice.  Being on a 1/2 or 2/3 die does not get the constitution
	*	bonus added to the hits, only whole dice get the bonus.
	*		In the character class the whole value represents hits and the fractionDice
	*	value represents halfHits for the character.
	***/
	public HitDice HitsAtLevel(int lvl)
	{
		HitDice actualHits = new HitDice();

		actualHits.whole = actualHits.fractional.fraction = 0;

		for (int i = 1; i <= lvl; i++)
		{   // Total up all the hits to this level based on what was rolled
			actualHits.whole += hitDieRolls[i].whole;
			actualHits.fractional.fraction += hitDieRolls[i].fractional.fraction;
		}   // for

		actualHits.whole += actualHits.fractional.fraction / 10;    // Add whole number from fractions
		actualHits.fractional.fraction = actualHits.fractional.fraction % 5;    // Leave any fraction that is left
		return actualHits;
	}   // HitsAtLevel(int lvl)

	/***
	*		Since we have to do this in multiple places it is implemented as a method we can call.
	*	This is only called in cases where we are rolling hits to complete a full die for the
	*	character.  This might be a fractional die going to a full die or it might be a full die.
	*	In either case, the complicating factor is if you have to subtract hits due to poor
	*	constituion.  In that case you can never have 0 hits for the roll, so we make sure that
	*	you get at least 1/2 hit for the roll.
	***/
	private HitDice AdjustByHitBonus(int dieRoll)
	{
		HitDice adjustedDieRoll = new HitDice();
		adjustedDieRoll.whole = (int)dieRoll;
		adjustedDieRoll.fractional.fraction = 0;

		if (bonusHits != 0)
		{   // Adjust hits based on rules for bonus hits for constitution - gets complicated
			if (bonusHits < 0)
			{   // With negative hits we can't get less than a 1 for the die roll
				if (bonusHits == -5)
				{   // Subtract 1/2 hits, which we can always do
					adjustedDieRoll.whole--;
					adjustedDieRoll.fractional.fraction = Fraction.oneHalf;
				}   // if
				else if (adjustedDieRoll.whole > 1)
				{   // Subtract 1 hit, as long as the rolled value is 2 or higher
					adjustedDieRoll.whole--;
				}   // else
			}   // if
			else
			{   // Add hits to the rolled amount
				if (bonusHits == 5)
					adjustedDieRoll.fractional.fraction = bonusHits;    // Add 1/2 hits
				else
					adjustedDieRoll.whole++;                // Add 1 hit
			}   // else
		}   // if

		return adjustedDieRoll;
	}   // AdjustByHitBonus(int dieRoll)

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
		HitDice diceToRoll = new HitDice();
		HitDice newHits = new HitDice();

		for (int i = 1; i <= level; i++)
		{   // Note that level 0 is always left at 0
			if (hitDieRolls[i].whole == 0)
			{   // We need to roll hits for this level, so get local copies last level and this one
				HitDice lastLevel = (HitDice)hitDice[i - 1];
				HitDice thisLevel = (HitDice)hitDice[i];

				newHits.whole = newHits.fractional.fraction = 0;
				diceToRoll.whole = thisLevel.whole - lastLevel.whole;
				diceToRoll.fractional.fraction = 0; // Default to this, but may be changed below
				if (lastLevel.fractional.fraction != thisLevel.fractional.fraction)
				{   // We are going up a fractional die to a different fraction or whole die
					if (thisLevel.fractional.fraction == 0)
					{   // We are going from a fractional die to a whole die, so figure the fraction
						if (lastLevel.fractional.fraction == 5)
							diceToRoll.fractional.fraction = 3; // We need to roll a 3-sided die for 1/2 die
						else
							diceToRoll.fractional.fraction = 2; // We need to roll a 2-sided die for 1/3 die

						newHits = AdjustByHitBonus(dice.RollDice(1, diceToRoll.fractional.fraction));
						if (diceToRoll.whole != 0)
							diceToRoll.whole--; // We are rolling up from a factional to a whole
					}   // if
					else
					{   // We don't adjust these hits, because they are only a fraction of a die.
						if (thisLevel.fractional.fraction == 5)
							diceToRoll.fractional.fraction = 3; // We need to roll a 3-sided die for 1/2 die
						else
							diceToRoll.fractional.fraction = 2; // We need to roll a 2-sided die for 1/3 die

						newHits.whole += (int)dice.RollDice(1, (int)diceToRoll.fractional.fraction);
					}   // if
				}   // if

				if (diceToRoll.whole != 0)
				{   // We have a change in whole dice since the previous level (may be more than 1)
					for (int j = 0; j < diceToRoll.whole; j++)
					{   // Roll each die and adjust it separately
						HitDice hitsRolled = AdjustByHitBonus(dice.RollDice(
																(int)diceToRoll.whole, 6));
						newHits.whole += hitsRolled.whole;
						newHits.fractional.fraction += hitsRolled.fractional.fraction;
					}   // for j
				}   // if

				if (newHits.fractional.fraction > 5)
				{   // It is possible that due to multiple hit die rolls and a bonus of 1/2 this is 10
					newHits.whole += newHits.fractional.fraction / 10;  // Add the whole numbers
					newHits.fractional.fraction %= 5;  // Remove the whole numbers
				}   // if

				hitDieRolls[i].whole = newHits.whole;
				hitDieRolls[i].fractional.fraction = newHits.fractional.fraction;
			}   // if
		}   // for

		hits = HitsAtLevel(level);
		deathMargin = (((double)hits.whole + ((double)hits.fractional.fraction) / 10)) *
					  ((double)constitution * 0.03d);   // Margin is hits* constitution* 0.03
	}   // RollHits()

	/***
	*		This method is called by the specific character classes with arrays that say how
	*	many hit dice this character gets for each level.  Be sure the arrays have maxLevel + 1
	*	elements in each of them.
	***/
	public void SetHitDice(int[] whole, int[] fractionalDie)
	{
		int maxHitsLevel = maxLevel + 1;

		if (whole.Length != fractionalDie.Length)
		{   // Catch the error of the two arrays being different sizes
			maxHitsLevel = (int)Math.Min(whole.Length, fractionalDie.Length);
			Debug.LogError("SetHitDices(): whole.Length is " + whole.Length +
				" and fractionalDie.Length is " + fractionalDie.Length);
		}   // if

		if (whole.Length > maxLevel + 1)
		{   // Catch the error of a passed array that is too long
			maxHitsLevel = maxLevel + 1;    // Force to be the max allowed
			Debug.LogError("SetHitDices(): wholeHitDice[] was longer than " +
				(maxLevel + 1).ToString());
		}   // if

		if (maxHitsLevel == maxLevel && whole.Length < maxLevel + 1)
		{   // Catch the error where we were not passed enough data in the arrays
			maxHitsLevel = (int)whole.Length;
			Debug.LogError("SetHitDices(): wholeHitDice[] was shorter than " +
				(maxLevel + 1).ToString());
		}   // if

		for (int i = 0; i < maxHitsLevel; i++)
		{   // Fill in the hitDice numbers for this character class
			if (hitDice[i] == null)
			{   // Only allocate a HitDie object the first time (should be the only time)
				hitDice[i] = new HitDice();
			}   // if

			hitDice[i].whole = whole[i];
			hitDice[i].fractional.fraction = fractionalDie[i];
		}   // for

		RollHits(); // Roll the hits once we know the hit dice.  The level must already be set
	}   // SetHitDice(int[] whole, int[] fractionalDice)

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
				}   // if
			}   // if
		}   // if

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
			case CharType.thief:            // Thief class character
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

	/***
	*		This will be written out later.  It will handle casting magic from items for any
	*	character type.  For now just testing using an Interface.
	***/
	public virtual void CastMagicSpell()
	{
		;
	}   // CastMagicSpell()

	/***
	*		This will be written out later.  It will handle casting cleric spells from items for any
	*	character type.  For now just testing using an Interface.
	***/
	public virtual void CastClericalSpell()
	{
		;
	}   // CastClericalSpell()

	/***
	*		This will be written out later.  It will handle taking a blow with a weapon for any
	*	character type.  For now just testing using an Interface.
	***/
	public virtual void TakeBlow()
	{
		;
	}   // TakeBlow()

	/***
	*		This will be written out later.  It will handle doing something thievish for any
	*	character type.  For now just testing using an Interface.
	***/
	public virtual void DoThievish()
	{
		;
	}   // DoThievish()
}   // class Character