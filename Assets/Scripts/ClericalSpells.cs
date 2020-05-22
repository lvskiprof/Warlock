using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*		This class defines all the known clerical spells.  Eventually it can have
*	methods added that will handle the effects, or those might be in derived
*	classes.  For now we are just defining some of the possible spells for each
*	possible spell level.  We have no spells past thirteenth (13th) level in Warlock.
***/

public class ClericalSpells : MonoBehaviour
{
	[SerializeField]
	public uint count;  // Number of spells
	[SerializeField]
	public enum Level0
	{   // Zeroth level clerical spells
		activateStaff
	};  // Level0
	[SerializeField]
	public enum Level1
	{   // First level clerical spells
		calmAnimals
	};  // Level1
	[SerializeField]
	public enum Level2
	{   // Second level clerical spells
		benediction
	};  // Level2
	[SerializeField]
	public enum Level3
	{   // Third level clerical spells
		animalControl
	};  // Level3
	[SerializeField]
	public enum Level4
	{   // Fourth level clerical spells
		circleOfHoliness
	};  // Level4
	[SerializeField]
	public enum Level5
	{   // Fifth level clerical spells
		antiMagicShell
	};  // Level5
	[SerializeField]
	public enum Level6
	{   // Sixth level clerical spells
		createAir
	};  // Level6
	[SerializeField]
	public enum Level7
	{   // Seventh level clerical spells
		detectUntrueAnswersAndStatements
	};  // Level7
	[SerializeField]
	public enum Level8
	{   // Eighth level clerical spells
		cureVeryCriticalWounds
	};  // Level8
	public enum Level9
	{   // Nineth level clerical spells
		restorePercentOfLifeLevels
	};  // Level9
	public enum Level10
	{   // Tenth level clerical spells
		cureDeadlyWounds
	};  // Level10
	public enum Level11
	{   // Eleventh level clerical spells
		anaerobicExistance
	};  // Level11
	public enum Level12
	{   // Twelveth level clerical spells
		Cure
	};  // Level12
	public enum Level13
	{   // Thirteenth level clerical spells
		restoreAllLifeLevels
	};  // Level13
}   // ClericalSpells