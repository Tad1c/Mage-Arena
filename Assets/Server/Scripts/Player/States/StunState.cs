using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class StunState : PlayerBaseState
{
    private Player _player;
    private IStateHelper _stateHelper;
    private float _timeLeft;

    // public StunState(float timeLeft, Player player)
    // {
    //     _player = player;
    //     _timeLeft = timeLeft;
    //     Invoke("CC", timeLeft);
    // }

    public StunState Init(float timeLeft, Player player)
    {
        _player = player;
       // _timeLeft = timeLeft;
        Invoke("CC", timeLeft);
        return this;
    }

    public void CC()
    {
        _player.StateHelper.RemoveState(this);
    }
    
    public override void StateUpdate(Player player)
    {
        
        //player.Move(0f, 0f);
        // _timeLeft -= Time.deltaTime;
        // if (_timeLeft <= 0)
        // {
        //     player.StateHelper.RemoveState(this);
        // }
        // Here we can check with StateHelper.HasState if some state might block sliding (ex. rooting)
        // StateHelper.HasState<RootState>() == true --> PopState()
    }
}