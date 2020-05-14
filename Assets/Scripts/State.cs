using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
	[SerializeField]
	public enum stateAction
	{   // These are actions to take when a state is entered.  The key is passed to the Action method
		defaultAction,		// Default action is to simply change to the indicated State
		moveToDungeon,		// Move the expedition party to the dungeon entrance
		buildParty,			// This causes the rest of the companions in your party to be created
		displayCharacter,	// This will display everything about the player character
		displayParty,		// This will allow the player to display all the party NPCs
		buildDungeonBadGuys,// This causes a party of bad guys to be built for a dungeon encounter
		buildSurfaceBadGuys,// This causes a party of bad guys to be built for a surface encounter
		returnToSavedState	// Return from current state to last saved state (might be a LIFO later)
	};

	[TextArea(1, 5)]
	[SerializeField]
	public string storyText;	// Text to output in storyText.text
	[TextArea(1, 3)]
	[SerializeField]
	string storyAreaText;		// Text to output in storyAreaText.text
	[SerializeField]
	public stateAction action;	// This is an action method that needs to be run for this State
	[SerializeField]
	public char[] responses;	// keys to select the next state in nextStates, 0 length if action-only state
	[SerializeField]
	public State[] nextStates;	// Array of next possible states,

	public string GetStateStory()
	{
		return this.storyText;
	}   // GetStateStory()

	public string GetStateStoryArea()
	{
		return this.storyAreaText;
	}   // GetStateStory()

	public State[] GetNextStates()
	{
		return this.nextStates;
	}   // GetNextStates()

	public char[] GetResponses()
	{
		return this.responses;
	}   // GetResponses()
}
