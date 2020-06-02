using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*		States come in two different basic forms:
*	#1:	States that interact with the user will have text in the interactionText
*		string.  This is a prompt to the user for what the options are for valid
*		responses to initiate an new state.  If there is an action to take, it
*		will be done after a valid response is given.  The response character is
*		passed to the action method being called in case it is needed.  In this
*		case the action should apply to all possible responses and they should
*		normally all proceed to the same state after the action method returns
*		with a true return value, showing that it was successful.  A response
*		character option of '*' will match any entered character.
*	#2:	Some states do not require any response.  They should have a state to
*		change to and may have an action method to be called prior to the state
*		being changed.  These methods will not have a response character passed
*		to them, since none was given.  The action routine does not return any
*		value, if there is an action method indicated, and it will always
*		proceed to the first (and only) state in the list of states.
*		
*		When the headerText, storyText, or interactionText strings are not empty
*	they will be displayed on the appropriate areas of the screen.  If you simply
*	want to blank one of these fields, the string should contain a space only.
*		The number of responses[] should match the number of nextStates[], except
*	when there are no responses[], in which case there should only be a single
*	state in nextStates[].
*		The description string is available for self documentation of what the
*	state is for in the Unity editor.  It can also be used as Debug.Log output to
*	show what the current state is.  This can be useful for tracing bad behavior.
*		The action value is used to show that an action method must be called
*	when the state either has a valid response or, if no response is required, as
*	soon as the state has been entered.
***/

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
/***
*		This is the only public item, other than the Get & Set methods, in this
*	class.  It enumerates the various actions so they can be set in the Unity
*	GUI and used in conditional statements to determine the action to perform.
***/

[SerializeField]
	public enum StateAction
	{   // These are actions to take when a state is entered.  The key is passed to the Action method
		defaultAction,      // Default action is to simply change to the indicated State
		exitGame,           // Used to exit the game by the user
		moveToDungeon,      // Move the expedition party to the dungeon entrance
		buildParty,         // This causes the rest of the companions in your party to be created
		displayCharacter,   // This will display everything about the player character
		displayParty,       // This will allow the player to display all the party NPCs
		buildDungeonBadGuys,// This causes a party of bad guys to be built for a dungeon encounter
		buildSurfaceBadGuys,// This causes a party of bad guys to be built for a surface encounter
		returnToSavedState  // Return from current state to last saved state (might be a LIFO later)
	};  // enum StateAction

	[SerializeField]
	string description="";  // Text to describe what this state is used for, for documentation use only
	[SerializeField]
	bool descriptionOutput = false;	// When set to false the description is output via Debug.Log()
	[TextArea(1, 1)]		// Define the size of this text field
	[SerializeField]
	string headerText="";   // Text to output in TextHeader.text if not empty
	[TextArea(1, 14)]		// Define the size of this text field
	[SerializeField]
	string storyText="";       // Text to output in storyText.text if not empty
	[TextArea(1, 5)]        // Define the size of this text field
	[SerializeField]
	string interactionText=""; // Text to output in interactionText.text if not empty
	[SerializeField]
	StateAction action;     // This is an action method that is executed after any text if output
	[SerializeField]
	char[] responses;       // keys to select the next state in nextStates, 0 length if action-only
							// state or automatically changes to a new state.  '*' is any key
	[SerializeField]
	State[] nextStates;     // Array of next possible states, set in Unity GUI

	/***
	*		This will get the string that represents the description for this state.
	*	It is only used for documenting in Unity what the purpose of the state is, but
	*	might be used in Debug.Log() output to show what state has been entered.
	***/
	public string GetStateDescription()
	{
		return this.description;
	}   // GetStateDescription()

	/***
	*		This will get the boolean flag that is used to show if we have output the
	*	description string for this state using Debug.Log().  If it is false (default)
	*	we need to output it and then set it to true.  If it is true we are still in
	*	the same state as before.  This is needed because Update() will be called
	*	multiple times while waiting for the user to do something that will change the
	*	state of the game.
	***/
	public bool GetStateDescriptionOutput()
	{
		return this.descriptionOutput;
	}   // GetStateDescriptionOutput()

	/***
	*		This will set the boolean flag to show if we have output the description
	*	string text or not using Debug.Log().  You use it to set it to false when the
	*	current state does not match the previous state and set it to true when you
	*	have output the description string text using Debug.Log().
	***/
	public void SetStateDescriptionOutput(bool outputFlag)
	{
		this.descriptionOutput = outputFlag;
	}   // SetStateDescriptionOutput()

	/***
	*		This will get the string that represents the headerText for this state.
	*	If it is empty nothing should be displayed.
	***/
	public string GetStateHeader()
	{
		return this.headerText;
	}   // GetStateHeader()

	/***
	*		This will set the string that represents the headerText for this state.
	*	If it is empty nothing will be displayed when this state is entered.
	***/
	public void SetStateHeader(string text)
	{
		this.headerText = text;
	}   // SetStateHeader()

	/***
	*		This will get the string that represents the storyText for this state.
	*	If it is empty nothing should be displayed.
	***/
	public string GetStateStory()
	{
		return this.storyText;
	}   // GetStateStory()

	/***
	*		This will set the string that represents the storyText for this state.
	*	If it is empty nothing will be displayed when this state is entered.
	***/
	public void SetStateStory(string text)
	{
		this.storyText = text;
	}   // SetStateStory()

	/***
	*		This will get the string that represents the interactionText for this state.
	*	If it is empty nothing should be displayed.
	***/
	public string GetStateInteraction()
	{
		return this.interactionText;
	}   // GetStateInteraction()


	/***
	*		This will set the string that represents the interactionText for this state.
	*	If it is empty nothing will be displayed when this state is entered.
	***/
	public void SetStateInteraction(string text)
	{
		this.interactionText = text;
	}   // SetStateInteraction()

	/***
	*		This will get the array of states to be used based on an entered response.
	*	If no responses are set for this state then it holds what the next state will be
	*	when this state has been completed.
	***/
	public State[] GetNextStates()
	{
		return this.nextStates;
	}   // GetNextStates()

	/***
	*		This returns the array of response characters.  It may be a length of 0, but
	*	if it is not then the array of nextStates should have the same length and each
	*	response character will match a corresponding state in this array.
	***/
	public char[] GetResponses()
	{
		return this.responses;
	}   // GetResponses()

	/***
	*		This will set the array of states to be used based on an entered response.
	*	If no responses are set for this state then it holds what the next state will be
	*	when this state has been completed.
	***/
	public void SetResponseAndNextStates(char[] responses, State[] states)
	{
		if (responses == null || responses.Length == 0)
		{   // Only allow this if there is a single state
			if (states.Length != 1)
			{   // Don't change anything and log that this is invalid (maybe throw exception later)
				Debug.LogError("SetResponsesAndNextStates() was passed a null argument for " +
					"responses and states had a length of " + states.Length +
					" when it should be only a length of 1.");
				return;
			}   // if
			else
				responses = new char[0];
		}   // if

		this.responses = responses;
		this.nextStates = states;
	}   // SetResponsesAndNextStates()

	/***
	*		This returns the action, if any, for this state.  The action is a Method that
	*	will be executed when the state has completed its processing.
	***/
	public StateAction GetAction()
	{
		return this.action;
	}   // GetAction()

	/***
	*		This returns the action, if any, for this state.  The action is a Method that
	*	will be executed when the state has completed its processing.
	***/
	public void SetAction(StateAction action)
	{
		this.action = action;
	}   // SetAction()
}   // class State