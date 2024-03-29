﻿using System.Collections;
using System.Collections.Generic;

/***
*		This class is used to represent various fractions we use in Warlock.  The fraction
*	is stored as a uint and represents a fraction.  This way we can easily create a string
*	with the proper fraction, but also turn it into the proper float or double value if it
*	is ever needed in that format.
***/

public class Fraction
{
	public int fraction;           // Here is where the number reprentsion the fraction is stored

	/***
	*		These are numeric constant that can be used to set fraction to supported fraction types.
	*	They are ordered from smallest to largest.
	****/
	public const int oneSeventySecond = 72;
	public const int oneSixtieth = 60;
	public const int oneSixth = 6;
	public const int oneThird = 1;
	public const int oneHalf = 5;
	public const int twoThirds = 2;
	public const int threeQuarters = 3;

	/***
	*		This is a default creator for this class
	***/

	public Fraction()
	{
		fraction = 0;
	}   // Fraction()

	/***
	*		This is a creator for this class where you can pass a uint to set the fraction.
	***/

	public Fraction(int frac)
	{
		fraction = frac;
	}   // Fraction(uint frac)

	/***
	*		This overrides the basic Object.ToString() method to implement one that knows how
	*	to turn the various uint values into a fraction.
	***/

	public override string ToString()
	{
		string value = "";
		int absFraction = System.Math.Abs(fraction);

		if (fraction < 0)
			value = "-";

		switch (absFraction)
		{   // Handle all the supported fraction types
			case 0:
				return "";
			case oneSeventySecond:
				return value + "1/72";
			case oneSixtieth:
				return value + "1/60";
			case oneSixth:
				return value + "1/6";
			case oneThird:
				return value + "1/3";
			case oneHalf:
				return value + "1/2";
			case twoThirds:
				return value + "2/3";
			case threeQuarters:
				return value + "3/4";
			default:
				return "?/??";   // Flag this as an unknown fraction
		}   // switch
	}   // ToString();

	/***
	*		This returns the fraction value as a float for doing math.
	***/
	public float ToFloat()
	{
		switch (fraction)
		{   // Handle all the supported fraction types
			case oneSeventySecond:
				return (float)1 / (float)72;
			case oneSixtieth:
				return (float)1 / (float)60;
			case oneSixth:
				return (float)1 / (float)6;
			case oneThird:
				return (float)1 / (float)3;
			case oneHalf:
				return (float)1 / (float)2;
			case twoThirds:
				return (float)2 / (float)3;
			case threeQuarters:
				return (float)3 / (float)4;
			default:
				return 0.0f;   // Flag this as an unknown fraction
		}   // switch
	}   // ToFloat()

	/***
	*		This returns the fraction value as a float for doing math.  Double provides
	*	much better precision than a float value.
	***/
	public double ToDouble()
	{
		switch (fraction)
		{   // Handle all the supported fraction types
			case oneSeventySecond:
				return (double)1 / (double)72;
			case oneSixtieth:
				return (double)1 / (double)60;
			case oneSixth:
				return (double)1 / (double)6;
			case oneThird:
				return (double)1 / (double)3;
			case oneHalf:
				return (double)1 / (double)2;
			case twoThirds:
				return (double)2 / (double)3;
			case threeQuarters:
				return (double)3 / (double)4;
			default:
				return 0.0d;   // Flag this as an unknown fraction
		}   // switch
	}   // ToDouble()

	/***
	*		This is an easy way to set the fraction by passing and integer.
	***/

	public void SetValue(int frac)
	{
		fraction = frac;
	}   // SetValue(int frac)
}   // class Fraction