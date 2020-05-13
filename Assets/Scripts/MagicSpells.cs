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

public class MagicSpells : MonoBehaviour
{
	[SerializeField]
	public uint count;  // Number of spells
	[SerializeField]
	public enum Level1
	{   // First level magic spells
		activateWand
	};  // Level1
	[SerializeField]
	public enum Level2
	{   // Second level magic spells
		activateStaff
	};  // Level2
	[SerializeField]
	public enum Level3
	{   // Third level magic spells
		ballLightning
	};  // Level3
	[SerializeField]
	public enum Level4
	{   // Fourth level magic spells
		acidResistance
	};  // Level4
	[SerializeField]
	public enum Level5
	{   // Fifth level magic spells
		airWalking
	};  // Level5
	[SerializeField]
	public enum Level6
	{   // Sixth level magic spells
		acidProtection
	};  // Level6
	[SerializeField]
	public enum Level7
	{   // Seventh level magic spells
		activateMagic
	};  // Level7
	[SerializeField]
	public enum Level8
	{   // Eighth level magic spells
		activateDetectors
	};  // Level8
}   // class MagicSpells
