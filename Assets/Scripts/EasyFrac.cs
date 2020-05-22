using System.Collections;
using System.Collections.Generic;

/***
*		This class is used to represent various whole numbers with fractions we use in Warlock.
*	The fraction is stored as a uint and represents a fraction.  This way we can easily create
*	a string with the proper fraction, but also turn it into the proper float or double value
*	if it is ever needed in that format.
*		This is typically needed for hit dice, where you have a whole number and a fraction of
*	1/2, 133, or 2/3.  It is also used for Hits and Spell Points where you may have a fraction
*	of 1/2.
*		It can also be used to store a pair of numbers that are related.  For example, with a
*	high strength you may need to multiply the number of dice of damage by one number and add
*	a second number time the number of normal damage dice a weapon would do.
***/

public class EasyFrac
{
	public int whole;			// Whole number portion of the number
	public Fraction fractional; // Fractional part of the number

	/***
	*		This is a default creator for this class
	***/

	public EasyFrac()
	{
		whole = 0;
		fractional = new Fraction(0);
	}   // EasyFrac()

	/***
	*		This is a creator for this class where you can pass two uint value to set the
	*	whole number and the fractional part.
	***/

	public EasyFrac(int wholeNumber, int frac)
	{
		whole = wholeNumber;
		fractional = new Fraction(frac);
	}   // EasyFrac(uint frac)

	/***
	*		This overrides the basic Object.ToString() method to implement one that knows how
	*	to turn the various uint values into a fraction.
	***/

	public override string ToString()
	{
		return whole.ToString() + " " + fractional.ToString();
	}   // ToString();

	/***
	*		This returns the fraction value as a float for doing math.  Probably never used.
	***/
	public float ToFloat()
	{
		return (float)whole + fractional.ToFloat();
	}   // ToFloat()

	/***
	*		This returns the fraction value as a float for doing math.  Double provides
	*	much better precision than a float value.  Probably never used.
	***/
	public double ToDouble()
	{
		return (double)whole + fractional.ToDouble();
	}   // ToDouble()
}   // class EasyFrac