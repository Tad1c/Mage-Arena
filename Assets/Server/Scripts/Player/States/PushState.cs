using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : PlayerBaseState
{

    private Vector3 direction;
    private float force;
    private float time;

    public override void EnterState(Player player)
    {
        Debug.Log("We are in AttackState");

        direction = player.projectileDirection;
        force = player.projectilePushForce;
        time = player.projectilePushTime;
        player.isHit = false;
    }

    public override void Update(Player player)
    {
        MoveAndPush(new Vector2(player.h, player.v), player);
    }

    private void MoveAndPush(Vector2 inputDirection, Player player)
    {
        player.Move(inputDirection);

        if (force >= 1)
        {
            // calculate the force to be applied to the rb
            player.Controller.AddForce(direction * force, ForceMode.VelocityChange);
            force = Mathf.Lerp(force, 0f, time * Time.deltaTime);
        }
        else
        {
            player.TransitionToState(player.moveState);
        }
    }
}

