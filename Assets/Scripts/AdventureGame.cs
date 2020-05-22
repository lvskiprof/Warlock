using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI TextHeader;    // James added this and it displays in the header area
	[SerializeField]
	private Text[] textList;    // This is a list of all our Text fields at run-time
	[TextArea(1, 8)]			// Define the size of this text field
	[SerializeField]
	Text textComponent;         // This is the upper text field, usually used to display information
	[TextArea(1, 5)]			// Define the size of this text field
	[SerializeField]
	Text textAreaComponent;		// This is the lower text field, usually use for entering responses
	[SerializeField]
	public State state;			// This is our current state
	[SerializeField]
	public State savedState;    // This is the state we need to return to after sub states play out
	[SerializeField]
	public Character _PC;       // This is the Player Character for the player
	[SerializeField]
	public Character[] _NPCs;   // These are the NonPlayer Characters for the player

	State[] nextStates;         // This is here for diagnostics when debugging


	/***
	 *		This are indexes to the various text fields, the order of which was found by
	 *	and error.  It can be eliminated if James can explain how he ties specific Text
	 *	definitions to the fields in the Unity GUI.
	***/
	private enum TextID
	{   // Index values into textComponents[] to get at the different text areas
		storyText,      // Story text where what is going on is displayed
		storyAreaText,  // Story area text where questions and responses are displayed
	};  // TextID

	/***
	 *		That is a state object that is used to contain a list of all possible states for the game.
	 *	The enum that follows is used to index into the nextStates array using the same name as the
	 *	State object in the Unity GUI.
	***/
	[SerializeField]
	public State allStates;     // This is a list of all states so the program can copy one into state
	public enum StateNames
	{
		a1_ExplainGame,
		a2_BuildParty,
		a3_PartyBuilt,
		a4_DisplayCharacter,
		a5_DisplayParty,
		a6_TravelToDungeon,
		a7_DungeonEntrance,
		a8_Room_1_FromEntrance,
		a9_Room_2_FromEntrance,
		a10_Room_1_FromRoom_2,
		a11_Room_2_FromRoom_1,
		a12_SurfaceEncounter,
		a13_DungeonEncounter,
		a14_ReturnToSavedState
	};	// enum StateNames

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
	private static AdventureGame instance;

	/***
	 *		Constructor for the class.  Is this really needed?
	***/
	private AdventureGame()
	{

	}	// AdventureGame()

	/***
	 *		This method populates the private instance reference the first time it is
	 *	called and can be used like this from any method to get access to methods in the
	 *	game instance:
	 *		AdventureGame.Instance.StoryText(GetClericInfo());
	 *			or
	 *		AdventureGame game = AdventureGame.Instance;
	 *		
	 *		Which one you use depends on if you want to just do a single thing (first case)
	 *	or call multiple methods from within your current method (second case).
	***/
	public static AdventureGame Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdventureGame)) as AdventureGame;

			return instance;
		}	// get
	}	// AdventureGame Instance

	/***
    *      This is the base creator that we need to use to access the public methods in this class.
    ***//*
	public AdventureGame()
	{   //Instance creator

	}   // AdventureGame()

	/***
     *      This method will output the string argument to the HeadingText field on the screen.
    ***/
	public void HeadingText(string text)
	{
		if (text.Length != 0)
			TextHeader.text = text;
	}   // HeadingText()

	/***
     *      This method will output the string argument to the StoryText field on the screen.
    ***/
	public void StoryText(string text)
	{
		if (text.Length != 0)
			textComponent.text = text;
			//textList[(int)TextID.storyText].text = text;
			//_TextStory.text = text;

	}   // StoryText()

	/***
     *      This method will output the string argument to the StoryAreaText field on the screen.
    ***/
	public void StoryAreaText(string text)
	{
		if (text.Length != 0)
			textAreaComponent.text = text;
			//textList[(int)TextID.storyAreaText].text = text;
			//_TextBody.text = text;
	}   // StoryAreaText()

	/***
     *      Method to handle processing Action methods.  Some states only have an action and then
     *  they move on to the next state.  Others require a valid response first before doing the
     *  action method.  Putting this as a separate method simplifies the code needed to handle actions.
    ***/
	public bool PerformAction(State state, char response)
	{
		bool validResponse = false;
		State.StateAction action = state.GetAction();

		switch (action)
		{
			case State.StateAction.defaultAction:
				validResponse = true;
				break;
			case State.StateAction.exitGame:
				Application.Quit();
				break;
			case State.StateAction.buildParty:
				validResponse = BuildParty.Instance.BuildExpeditionParty(response);
				StartCoroutine(DelayForStoryText(3));
				DelayForStoryText(3);
				break;
			case State.StateAction.displayCharacter:
				switch(_PC.charClass)
				{
					case Character.CharType.cleric:
						((Cleric)_PC).GetCharacterInfo();
						break;
					case Character.CharType.dwarf:
						((Dwarf)_PC).GetCharacterInfo();
						break;
					case Character.CharType.elf:
						((Elf)_PC).GetCharacterInfo();
						break;
					case Character.CharType.fighter:
						((Fighter)_PC).GetCharacterInfo();
						break;
					case Character.CharType.mage:
						((Mage)_PC).GetCharacterInfo();
						break;
					case Character.CharType.thief:
						((Thief)_PC).GetCharacterInfo();
						break;
				}   // switch

				break;
			case State.StateAction.displayParty:
				break;
			case State.StateAction.moveToDungeon:
				// Yet to be written, but state will be changed to nextStates[0]
				validResponse = true;
				break;
			case State.StateAction.buildSurfaceBadGuys:
				break;
			case State.StateAction.buildDungeonBadGuys:
				break;
			case State.StateAction.returnToSavedState:
				state = savedState;
				savedState = null;
				break;
			default:
				// Handle the case where we have defined an action but not written the code
				StoryAreaText("Unknown state " + action.ToString() + " encountered.");
				break;
		}   // switch

		return validResponse;
	}   // PerformAction()

	/***
     *      Method asked for in the class, but not really used here.  Update() handles managing the
     *  current state and moving to the next state for now.  So may want to remove this later or
     *  move the code from Update() into here.
    ***/
	private void ManageState()
	{
		nextStates = state.GetNextStates();
	}

	/***
	 *		DelayForStoryText() is used to create a delay of <delay> seconds.  Useful if you
	 *	don't want some test you just displayed to go away before it can be read by the user.
	***/
	public IEnumerator DelayForStoryText(Int32 delay)
	{
		yield return new WaitForSeconds(delay);
	}   // DelayForStoryText()

	/***
	 *      Start is called before the first frame update
	***/
	void Start()
	{
		nextStates = state.GetNextStates();
		textList = GameObject.FindObjectsOfType<Text>();
		StoryText(state.GetStateStory());
		StoryAreaText(state.GetStateStoryArea());
	}   // Start()

	/***
     *      Update is called once per frame
    ***/
	void Update()
	{
		int i;
		string validResponses = "";
		char[] responses;
		string response = "";
		const string pleaseEnter = "Please enter in only one character from the list: ";

		responses = state.GetResponses();

		for (i = 0; i < responses.Length; i++)
		{   // Build up a single string with all the valid response characters in it
			validResponses += responses[i];
		}   // for

		if (responses.Length != 0)
		{   // Check for an input character
			response = Input.inputString;
		}   // if

		if (response.Length > 1)
		{   // Reject anything that is not a single character response
			textList[(int)TextID.storyText].text = pleaseEnter + validResponses;
		}   // if
		else if (response.Length == 0 && responses.Length != 0)
		{   // Ignore a length of 0
			;
		}   // else-if
		else
		{   // Convert to upper case and check the character based on the list in validResponses
			bool validResponse = false; // Default to false

			if (responses.Length == 0)
			{   // Handle the case where no response is needed, but there is an action to perform
				i = 0;  // For action-only states the next state is always element 0
				validResponse = PerformAction(state, (char)0);
			}   // if
			else
			{
				response = response.ToUpper();

				for (i = 0; !validResponse && i < responses.Length; i++)
				{   // Check for a valid response or execute action if no response required
					if (responses[i] == '*' || response.ToCharArray()[0] == responses[i])
					{   // We found a valid response ('*' == any key), now call any action function
						if (state.GetAction() != State.StateAction.defaultAction)
							validResponse = PerformAction(state, responses[i]);
						else
							validResponse = true;

						break;  // Exit for loop
					}   // if
				}   // for
			}   // else

			if (!validResponse)
			{
				textList[(int)TextID.storyText].text = pleaseEnter + validResponses;
			}   // if
			else
			{   // Change to the next state and output appropriate text (text areas may be blanked)
				state = state.GetNextStates()[i];
				StoryText(state.GetStateStory());
				StoryAreaText(state.GetStateStoryArea());
			}   // else
		}   // else

		ManageState();
	}   // Update()
}   // class AdventureGame