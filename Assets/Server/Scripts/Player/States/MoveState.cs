using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("We are in MoveState");
    }

    public override void Update(Player player)
    {
        if (player.isHit)
            player.TransitionToState(player.pushState);
        
        player.Move(new Vector2(player.h, player.v));
    }
}
