using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/***
*		This class defines all the known magic spells.  Eventually it can have
*	methods added that will handle the effects, or those might be in derived
*	classes.  For now we are just defining some of the possible spells for each
*	possible spell level.  We have no spells past eighth (8th) level in Warlock.
***/

public class MagicSpells
{
	[SerializeField]
	public uint cost;               // Spell point cost of a spell
	[SerializeField]
	public MagicClass magicClass;   // The magic class for this spell
	[SerializeField]
	public string spellName;        // Name of the magic spell in text
	[SerializeField]
	public Spells spell;            // Enumerated spell name
	[SerializeField]
	public Shapes shape;            // Shape of the spell
	[SerializeField]
	public uint duration;           // How long the spell lasts (0 means instant)
	[SerializeField]
	public Interval interval;       // What measure of time to use for duration
	[SerializeField]
	public bool maintained;         // True if this is a maintained spell
	[SerializeField]
	public uint costToMaintain;     // How many spell points per interval to maintain the spell
	[SerializeField]
	public Interval intervalPeriod; // The period do you need to expend the maintenance cost
	[SerializeField]
	public uint[] affectedAmount = new uint[2]; // Number of dice and number of sides on dice to roll
	[SerializeField]
	public uint[] areaSize = new uint[3];   // Length, Width, height/angle
	[SerializeField]
	public bool vari;               // True if this is variable sized spell (area size is maximum)
	[SerializeField]
	public enum Spells
	{   // This will be a list of all magic spells, split for ease of reading by level (fill in later)
		/***
		*		First level magic spells
		***/
		activateWand,
		/***
		*		Second level magic spells
		***/
		activateStaff,
		/***
		*		Third level magic spells
		***/
		ballLightning,
		/***
		*		Fourth level magic spells
		***/
		acidResistance,
		/***
		*		Fifth level magic spells
		***/
		airWalking,
		/***
		*		Sixth level magic spells
		***/
		acidProtection,
		/***
		*		Seventh level magic spells
		***/
		activateMagic,
		/***
		*		Eighth level magic spells
		***/
		activateDetectors
	};  // enum Spells
	public enum MagicClass
	{
		earth,      // Earth, Body, Inanimate magic
		fire,       // Fire and destructive magic
		will,       // Personal will and general 'Magical' effect magic
		forces,     // Outside forces, spirits, and detection magic
		cold,       // Cold, water, and life magic
		air         // Air, electricity, light, and heat magic
	}   // enum MagicClass
	public enum Shapes
	{   // All possible shapes of spells (there are more, but this will do for now)
		point,          // Microballs and individual spells
		sixBySix,       // 6"x6" square (Sleep)
		bolt,           // Standard 6" bolt
		ball,           // Standard 2" radius ball
		cone,           // Standard 6" 30-degree cone
		wall,
		beam            // Beam shape
	}   // enum Shapes
	public enum Interval
	{   // Time intervals (may want to move this to Character or AdventureGame)
		phase,          // One Phase = 10 seconds
		turn            // One Turn = 6 Phases, or one minute
	}   // enum Interval

	/***
    *		This is the base creator that we need to use to access the public
    *	methods in this class.
    ***/
	public MagicSpells()
	{
	}   // MagicSpells()
}   // class MagicSpells