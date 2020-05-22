using System.Collections;
using System.Collections.Generic;

public class HitDice : EasyFrac
{
	public int wholeDice;
	public int fractionDie;

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