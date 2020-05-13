using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
	[SerializeField] private Text[] textList;
	[SerializeField] Text textComponent;
	[SerializeField] Text textAreaComponent;
	[SerializeField] State state;
	State[] nextStates;

	private enum textID
	{   // Index values into textComponents[] to get at the different text areas
		storyText,      // Story text where what is going on is displayed
		storyAreaText,  // Story area text where questions and responses are displayed
		headingText     // Header text area
	};

	/***
    *      This is the base creator that we need to use to access the public methods in this class.
    ***/
	public AdventureGame()
	{   //Instance creator

	}   // AdventureGame()

	/***
     *      This method will output the string argument to the HeadingText field on the screen.
    ***/
	public void HeadingText(string text)
	{
		if (text.Length != 0)
			textList[(int)textID.headingText].text = text;
	}   // HeadingText()

	/***
     *      This method will output the string argument to the StoryText field on the screen.
    ***/
	public void StoryText(string text)
	{
		if (text.Length != 0)
			textList[(int)textID.storyText].text = text;
	}   // StoryText()

	/***
     *      This method will output the string argument to the StoryAreaText field on the screen.
    ***/
	public void StoryAreaText(string text)
	{
		if (text.Length != 0)
			textList[(int)textID.storyAreaText].text = text;
	}   // StoryAreaText()

	/***
     *      Method to handle processing Action methods.  Some states only have an action and then
     *  they move on to the next state.  Others require a valid response first before doing the
     *  action method.  Putting this as a separate method simplifies the code needed to handle actions.
    ***/
	private bool PerformAction(State state, char response)
	{
		bool validResponse = false;

		switch (state.action)
		{
			case State.stateAction.defaultAction:
				validResponse = true;
				break;
			case State.stateAction.buildParty:
				BuildParty buildParty = new BuildParty();
				validResponse = buildParty.BuildExpeditionParty(this, response);
				break;
			case State.stateAction.buildBadGuys:
				break;
			case State.stateAction.moveToDungeon:
				// Yet to be written, but state will be changed to nextStates[0]
				validResponse = true;
				break;
			default:
				// Handle the case where we have defined an action but not written the code
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
	 *      Start is called before the first frame update
	***/
	void Start()
	{
		nextStates = state.nextStates;
		//nextStates = state.GetNextStates();
		Debug.Log("nextStates has " + nextStates.Length + " elements in the array");
		textList = GameObject.FindObjectsOfType<Text>();
		StoryText(state.GetStateStory());
		StoryAreaText(state.GetStateStoryArea());

		foreach (var item in nextStates)
		{   // Display the name of each item that was found
			Debug.Log("item name: " + item.name);
		}   // foreach
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
			textList[(int)textID.storyText].text = pleaseEnter + validResponses;
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
					if (response.ToCharArray()[0] == state.responses[i])
					{   // We found a valid response, now call any action function
						if (state.action != State.stateAction.defaultAction)
							validResponse = PerformAction(state, state.responses[i]);
						else
							validResponse = true;

						break;  // Exit for loop
					}   // if
				}   // for
			}   // else

			if (!validResponse)
			{
				textList[(int)textID.storyText].text = pleaseEnter + validResponses;
			}   // if
			else
			{   // Change to the next state and output appropriate text (text areas may be blanked)
				state = state.nextStates[i];
				StoryText(state.GetStateStory());
				StoryAreaText(state.GetStateStoryArea());
			}   // else
		}   // else

		ManageState();
	}   // Update()
}   // class AdventureGame