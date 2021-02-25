using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**  
Slide state pushes the player in the direction of the hit,
but the player is still able to move, whilst a force is being added

- Sliding can be interuppted by rooting effects
- Player can slide while being stunned
*/
public class SlideState : PlayerBaseState
{

    private Vector3 _direction;
    private float _force;
    private float _time;

    public float Force
    {
        get => _force;
    }

    public SlideState(Vector3 direction, float force, float time)
    {
        _direction = direction;
        _force = force;
        _time = time;
    }

    public override void StateUpdate(Player player)
    {
        
     //   MyLog.D("SlideState is called");

        if (!player.StateHelper.HasState<StunState>())
        {
            player.Move(new Vector2(player.h, player.v));
        }
        else
        {
            player.Move(new Vector2(0f, 0f));
        }

        // Here we can check with StateHelper.HasState if some state might block sliding (ex. rooting)
        // StateHelper.HasState<RootState>() == true --> PopState()

        if (_force >= 1)
        {
            // calculate the force to be applied to the rb
            player.Controller.AddForce(_direction * _force, ForceMode.VelocityChange);
            _force = Mathf.Lerp(_force, 0f, _time * Time.deltaTime);
        }
        else
        {
            player.StateHelper.RemoveState(this);
        }

    }
}

