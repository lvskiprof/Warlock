using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
	/***
    *		Roll numDice dice with numSides sides and return the resulting value.
    *	Number returned is from 1 to S.  Before each die roll we throw away from 1
    *   to 10 random numbers.  Doing this makes even bad random number generators
    *   givegood random results.
    *   
    *       I previously found that random numbers generated on a RSTS/E DEC PDP/11
    *   system I used in college could never generate a 3 or 18 even after 1000
    *   rolls of three 6-sided dice.  The distribution curve for 1000 numbers also
    *   did not give the curve you would expect to see.  By making a change like
    *   this I was able to get the correct distribution curve.  Many RNGs, while
    *   their results may give a "random" distribution, rarely seem to generate
    *   a number of the same magnitude as the next number.  While it should be
    *   rare, it should still be possible to flip a coin and have it come up heads
    *   10 times in a row.  Doing this change seems to address that issue.
    *   
    *       Copyright (C) Michael Robert Riley 1976
   ***/
	public int RollDice(int numDice, int numSides)
	{
		int total = 0;
		int die;
		int sidesPlusOne = numSides + 1;

		if (numDice > 100)
		{   // Don't allow numDice to be super large due to a math error, as that will take too long
			Debug.LogError("RollDice(): NumDice was " + numDice);
			numDice = System.Math.Min(100, numDice);
		}   // if

		for (int i = 0; i < numDice; i++)
		{
			int gigo = Random.Range(1, 11);	// This can be a constant number if you want

			for (int j = 0; j < gigo; j++)
			{   // Throw away from 1 to 10 random numbers to ensure true randomness
				die = Random.Range(1, 11);
			}   // for

			die = Random.Range(1, sidesPlusOne);	// Use this value as the die roll
			total += die;
		}   // for

		return total;
	}   // RollDice()

	/***
	*      This is the creator that we need to use to access the public methods
	*  in this class.
	***/
	public Dice()
	{   //Instance creator

	}   // Dice()
}   // class Dice