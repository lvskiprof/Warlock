using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
	/***
	 *		This is the only public item, other than the Get methods, in this class.
	 *	It enumerates the various actions so they can be set in the Unity GUI and
	 *	used in conditional statements to determine the action to perform.
	***/

	[SerializeField]
	public enum StateAction
	{   // These are actions to take when a state is entered.  The key is passed to the Action method
		defaultAction,		// Default action is to simply change to the indicated State
		moveToDungeon,		// Move the expedition party to the dungeon entrance
		buildParty,			// This causes the rest of the companions in your party to be created
		displayCharacter,	// This will display everything about the player character
		displayParty,		// This will allow the player to display all the party NPCs
		buildDungeonBadGuys,// This causes a party of bad guys to be built for a dungeon encounter
		buildSurfaceBadGuys,// This causes a party of bad guys to be built for a surface encounter
		returnToSavedState	// Return from current state to last saved state (might be a LIFO later)
	};	// enum StateAction

	//[TextArea(1, 5)]
	[SerializeField]
	string storyText;	// Text to output in storyText.text if not empty
	//[TextArea(1, 3)]
	[SerializeField]
	string storyAreaText;// Text to output in storyAreaText.text if not empty
	[SerializeField]
	StateAction action;	// This is an action method that is executed after any text if output
	[SerializeField]
	char[] responses;	// keys to select the next state in nextStates, 0 length if action-only state or automatically changes to a new state
	[SerializeField]
	State[] nextStates;	// Array of next possible states, set in Unity GUI

	/***
	 *		This will get the string that represents the storyText for this state.
	 *	If it is empty nothing should be displayed.
	***/
	public string GetStateStory()
	{
		return this.storyText;
	}   // GetStateStory()

	/***
	 *		This will get the string that represents the storyAreaText for this state.
	 *	If it is empty nothing should be displayed.
	***/
	public string GetStateStoryArea()
	{
		return this.storyAreaText;
	}   // GetStateStory()

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
	 *		This returns the action, if any, for this state.  The action is a Method that
	 *	will be executed when the state has completed its processing.
	***/
	public StateAction GetAction()
	{
		return action;
	}	// GetAction()
}	// class State
