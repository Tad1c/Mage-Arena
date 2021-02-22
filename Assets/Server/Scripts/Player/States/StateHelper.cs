using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateHelper : IStateHelper
{
    private readonly List<PlayerBaseState> playerStates = new List<PlayerBaseState>();

    public StateHelper()
    {
        // add the default state for the player
        // this should't be removed, but will always be pushed on bottom
        playerStates.Add(new MoveState());
        LogCurrentStates();
    }

    public void AddState(PlayerBaseState state)
    {
        if (state is MoveState)
        {
            MyLog.D("MoveState is not added, because it exist");
            return;
        }

        MyLog.D("Pushing state " + state.GetType().Name);

        if (HasState(state))
            RemoveState(state);

        // MoveState -> SlideState -> StunState

        // to refresh the state, just pop the existing state and add the new one

        // insert new state on top, pushing the move state to bottom
        playerStates.Insert(0, state);

        LogCurrentStates();
    }

    public void RemoveState(PlayerBaseState stateToPop)
    {
        // Make sure not to pop a MoveState
        
        if (stateToPop is MoveState) return;

        MyLog.D("Popping " + stateToPop.GetType().Name);

        foreach (var state in playerStates)
        {
            if (state.GetType() == stateToPop.GetType())
            {
                playerStates.Remove(state);
                MyLog.D("Popped");
                break;
            }
        }
        LogCurrentStates();
        
        // if(playerStates.Count == 0)
        //     playerStates.Add(new MoveState());
    }

    private void LogCurrentStates()
    {
        MyLog.D(StatesCount() + " states in stack: ");

        foreach (var state in playerStates)
        {
            MyLog.D("  --  " + state.GetType().ToString());

        }
    }

    public bool HasState(PlayerBaseState hasThisState)
    {
        foreach (var state in playerStates)
        {
            if (state.GetType() == hasThisState.GetType()) return true;
        }
        return false;
    }


    public PlayerBaseState GetTopState() => playerStates[0];

    public bool HasState<T>() => playerStates.OfType<T>().Any();

    public int StatesCount() => playerStates.Count;

}
