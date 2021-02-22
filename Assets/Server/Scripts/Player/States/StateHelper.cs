using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHelper
{

    public StateHelper() {
        // add the default state for the player
        // this should't be removed, but will always be pushed on bottom
        playerStates.Add(new MoveState());
        MyLog.D("StateHelper initialized with " + StatesCount() + " states");
    }

    private readonly List<PlayerBaseState> playerStates = new List<PlayerBaseState>();

    public void PushState(PlayerBaseState state)
    {
        MyLog.D("Pushing state " + state.GetType().Name);

        //TODO: add logic to refresh certain states instead of adding them as duplicates
        // insert new state on top, pushing the move state to bottom
        if (playerStates.Contains(state)) return;
        playerStates.Insert(0, state);

        MyLog.D("State pushed");
    }

    public void PopState()
    {
        MyLog.D("Poping " + playerStates[0].GetType().Name);

        // Is there bettwe way to do this?
        // We don't want to remove MoveState if it's on top
        if (playerStates[0] is MoveState) return;


        playerStates.RemoveAt(0);

        MyLog.D("Popped");
        MyLog.D(StatesCount() + " states in stack");
    }

    public PlayerBaseState GetTopState() => playerStates[0];

    public bool HasState(PlayerBaseState state) => playerStates.Contains(state);

    public int StatesCount() => playerStates.Count;

}
