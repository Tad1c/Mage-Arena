using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerBaseState
{

    private Vector3 direction;
    private float force;
    private float time;

    public override void EnterState(Player player)
    {
        Debug.Log("We are in AttackState");

        direction = player.Projectile.direction;
        force = player.Projectile.force;
        time = player.Projectile.time;
        player.isHit = false;
    }

    public override void Update(Player player)
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        MoveAndPush(new Vector2(h, v), player);
    }

    private void MoveAndPush(Vector2 inputDirection, Player player)
    {
        player.Move(inputDirection);

        if (force >= 1)
        {
            // calculate the force to be applied to the rb
            player.Controller.AddForce(-player.transform.forward * force, ForceMode.VelocityChange);
            force = Mathf.Lerp(force, 0f, time * Time.deltaTime);
        }
        else
        {
            player.TransitionToState(player.moveState);
        }
    }
}

