public class HitDice : EasyFrac
{
	/***
	*		This is a default creator for this class
	***/

	public HitDice()
	{
		whole = 0;
		fractional.fraction = 0;
	}   // EasyFrac()

	/***
	*		This is a creator for this class where you can pass two int values to set the
	*	whole number and the fractional part.
	***/
	public HitDice(int wholeNumber, int frac)
	{
		whole = wholeNumber;
		fractional.fraction = frac;
	}   // EasyFrac(uint frac)

	/***
	*		When you have a 1/2 or 2/3 hit die the "total" hit dice is considered to be
	*	the number of whole dice plus one.  This is used for figuring defense and attack
	*	levels as well as certain effects that are hit die level-based.
	***/
	public int TotalHitDice()
	{
		if (fractional.fraction == 5 || fractional.fraction == 2)
			return whole + 1;
		else
			return whole;
	}   // TotalHitDice()
}   // class HitDice