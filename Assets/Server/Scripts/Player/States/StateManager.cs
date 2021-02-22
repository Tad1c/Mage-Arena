using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour, IStateHelper
{

    private StateHelper stateHelper;

    public PlayerBaseState playerState
    {
        get;
        set;
    }

    private void Awake()
    {
        stateHelper = new StateHelper(this);
    }

    public void AddState(PlayerBaseState state)
    {
        stateHelper.PushState(state);
    }

    public void RemoveState(PlayerBaseState state)
    {
        stateHelper.PopState(state);
    }

    public PlayerBaseState GetTopState() => stateHelper.GetTopState();

}
