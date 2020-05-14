using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BuildParty : MonoBehaviour
{
    /***
     *      This is the creator that we need to use to access the public methods
     *  in this class.
    ***/
    public BuildParty()
    {   //Instance creator

    }   // BuildParty()

    /***
     *      This method will create a Mage character.
    ***/
    void CreateMage(AdventureGame game)
	{

	}   // CreateMage()

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
			//Thread.Sleep(3 * 1000); // Sleep for 3 seconds so text can be read

		}   // if

		return valid;
	}   // BuildExpeditionParty()
}
