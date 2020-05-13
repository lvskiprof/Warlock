using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
	[SerializeField]
	public enum stateAction
	{   // These are actions to take when a valid key is entered.  The key is passed to the Action method
		defaultAction,          // Default action is to simply change to the indicated State
		moveToDungeon,          // Move the expedition party to the dungeon entrance
		buildParty,             // This causes the rest of the companions in your party to be created
		buildBadGuys            // This causes a party of bad guys to be built for an encounter
	};

	[TextArea(1, 5)]
	[SerializeField]
	string storyText;       // Text to output in storyText.text
	[TextArea(1, 3)]
	[SerializeField]
	string storyAreaText;  // Text to output in storyAreaText.text
	[SerializeField]
	public stateAction action;          // 
	[SerializeField]
	public char[] responses;        // keys to select the next state in nextStates, 0 length if action-only state
	[SerializeField]
	public State[] nextStates;      // Array of next possible states,

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
