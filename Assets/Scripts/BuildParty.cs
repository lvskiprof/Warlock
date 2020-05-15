using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BuildParty : MonoBehaviour
{

	/***
	 *		This is a Singleton value that has the instance of the game stored in it by
	 *	the Instance method below.  It is stored so we only have the overhead of finding
	 *	it the first time we ask for it.
	 *		If you declare a variable like this:
	 *			AdventureGame game;
	 *		The declaration will call the public static AdventureGame Instance method and
	 *	set the object reference to the instance of the game stored here.  You can then
	 *	use it to reference and public values or mathods and be sure they will be using
	 *	the actual game instance.
	 *		Here is some documentation on this:
	 *			https://answers.unity.com/questions/891380/unity-c-singleton.html
	***/
	private static BuildParty instance;

	/***
	 *		Constructor for the class.  Is this really needed???
	***/
	private BuildParty()
	{

	}   // BuildParty()

	/***
	 *		This method populates the private instance reference the first time it is
	 *	called and can be used like this from any method to get access to methods in the
	 *	game instance:
	 *		BuildParty.Instance.StoryText(GetMageInfo());
	 *			or
	 *		BuildParty buildParty = BuildParty.Instance;
	 *		
	 *		Which one you use depends on if you want to just do a single thing (first case)
	 *	or call multiple methods from within your current method (second case).
	***/
	public static BuildParty Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(BuildParty)) as BuildParty;

			return instance;
		}	// get
	}	// BuildParty Instance

	/***
     *      This method will build your character and the NPCs that make up your expedition party.
    ***/

	public bool BuildExpeditionParty(char response)
	{
		const string selected = "You have selected ";
		const string yourCharacter = " as your character.";
		bool valid = true;  // Default to true
		Character PC;
		AdventureGame game = AdventureGame.Instance;

		switch (response)
		{
			case 'M':
				game.StoryAreaText(selected + "a Mage" + yourCharacter);
				PC = new Mage();
				break;
			case 'F':
				game.StoryAreaText(selected + "a Fighter" + yourCharacter);
				break;
			case 'C':
				game.StoryAreaText(selected + "a Cleric" + yourCharacter);
				break;
			case 'T':
				game.StoryAreaText(selected + "a Thief" + yourCharacter);
				break;
			case 'D':
				game.StoryAreaText(selected + "a Dwarf" + yourCharacter);
				break;
			case 'E':
				game.StoryAreaText(selected + "an Elf" + yourCharacter);
				break;
			default:    // Should never be reached, because update handles validating input, but allow for switch not handling all cases the State allows for
				game.StoryAreaText("You have entered an illegal character.  " +
					"Please enter in only one character from the list: MFCTDE");
				valid = false;  // Let the caller know that this is not a valid selection
				break;
		}   // switch

		if (valid)
		{   // Create this character and the rest of the party and go to the next state
			game.DelayForStoryText(3); // Sleep for 3 seconds so text can be read

		}   // if

		return valid;
	}   // BuildExpeditionParty()
}
