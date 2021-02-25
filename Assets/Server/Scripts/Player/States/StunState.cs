using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class StunState : PlayerBaseState
{
    private float _timeLeft;

    public StunState(float timeLeft)
    {
        _timeLeft = timeLeft;
    }

    public override void StateUpdate(Player player)
    {
        
        //MyLog.D("StunState is called");

        _timeLeft -= Time.deltaTime;
        
        if (_timeLeft <= 0)
            player.StateHelper.RemoveState(this);
    }
}