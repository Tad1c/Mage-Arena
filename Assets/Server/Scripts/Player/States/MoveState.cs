using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerBaseState
{
    public override void Update(Player player)
    {
        // Here we can check with StateHelper.HasState if some state might block moving (ex. stun)
        // StateHelper.HasState<StunState>() == true --> return;

        player.Move(new Vector2(player.h, player.v));
    }
}
