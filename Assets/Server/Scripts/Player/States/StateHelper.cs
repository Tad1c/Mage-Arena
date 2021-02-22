using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateHelper
{
    private readonly List<PlayerBaseState> playerStates = new List<PlayerBaseState>();

    private readonly IStateHelper _stateHelper;

    public StateHelper(IStateHelper stateHelper)
    {
        _stateHelper = stateHelper;
        // add the default state for the player
        // this should't be removed, but will always be pushed on bottom
        MoveState defaultMoveState = new MoveState();
        playerStates.Add(defaultMoveState);

        _stateHelper.playerState = defaultMoveState;

        LogCurrentStates();
    }

    public void PushState(PlayerBaseState state)
    {
        MyLog.D("Pushing state " + state.GetType().Name);

        //TODO: add logic to refresh certain states instead of adding them as duplicates, and this COntains won't work
        //TODO: we need a way to check if certain type of PlayerBaseState exists in playerStates, ex: HasState(SlideState)
        if (HasState(state)) return;

        // MoveState -> SlideState -> StunState

        // to refresh the state, just pop the existing state and add the new one

        // insert new state on top, pushing the move state to bottom
        playerStates.Insert(0, state);
        _stateHelper.playerState = state;

        LogCurrentStates();
    }

    public void PopState(PlayerBaseState stateToPop)
    {
        // Make sure not to pop a MoveState
        if (stateToPop is MoveState) return;

        MyLog.D("Popping " + stateToPop.GetType().Name);

        foreach (PlayerBaseState state in playerStates)
        {
            if (state.GetType().Equals(stateToPop.GetType()))
            {
                playerStates.Remove(state);
                MyLog.D("Popped");
                break;
            }
        }

        _stateHelper.playerState = playerStates[0];

        LogCurrentStates();
    }

    private void LogCurrentStates()
    {
        MyLog.D(StatesCount() + " states in stack: ");

        foreach (PlayerBaseState state in playerStates)
        {
            MyLog.D("  --  " + state.GetType().ToString());

        }
    }

    public bool HasState(PlayerBaseState hasThisState)
    {
        foreach (PlayerBaseState state in playerStates)
        {
            if (state.GetType().Equals(hasThisState.GetType())) return true;
        }
        return false;
    }


    public PlayerBaseState GetTopState() => playerStates[0];

    public bool HasState<T>() => playerStates.OfType<T>().Any();

    public int StatesCount() => playerStates.Count;

}
