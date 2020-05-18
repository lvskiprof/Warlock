using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDice
{
	public int wholeDice;		// Whole die count for this level of hit dice
	public int fractionDie;		// Either 5 for 1/2, 1 for 1/3, or 2 for 2/3

	/***
	 *      This is the creator that we need to use to access the public methods
	 *  in this class.
	***/
	public HitDice()
	{   //Instance creator

	}   // HitDice()

	/***
	 *		When you have a 1/2 or 2/3 hit die the "total" hit dice is considered to be
	 *	the number of whole dice plus one.  This is used for figuring defense and attack
	 *	levels as well as certain effects that are hit die level-based.
	***/
	public int TotalHitDice()
	{
		if (fractionDie == 5 || fractionDie == 2)
			return wholeDice + 1;
		else
			return wholeDice;
	}   // TotalHitDice()
}   // class HitDice
