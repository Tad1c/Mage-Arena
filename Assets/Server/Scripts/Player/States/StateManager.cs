using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
    pushed,
    stunned,
    pushedAndStunned
}

public class StateManager : MonoBehaviour
{


    private PlayerBaseState _currentState;
    
    public readonly MoveState moveState = new MoveState();
    public readonly PushState pushState = new PushState();

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        TransitionToState(moveState);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(_player);
    }

    private void Update()
    {
        
        
        
        
        
        
        _currentState.Update(_player);
    }
    
    
}
