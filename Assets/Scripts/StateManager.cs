using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum StateView { Default, GetStateStory, GetStateStoryArea, GetNextState, GetResponses};

    public StateView _CurrentState = StateView.Default;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _CurrentState = StateView.Default;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _CurrentState = StateView.GetStateStory;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _CurrentState = StateView.GetStateStoryArea;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _CurrentState = StateView.GetNextState;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _CurrentState = StateView.GetResponses;
        }
    }
}
