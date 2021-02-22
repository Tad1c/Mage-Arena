using System.Collections;
using UnityEngine;


public class MockState : IStateHelper
{
    private PlayerBaseState playerBaseState;
    public PlayerBaseState playerState
    {
        get => playerBaseState;
        set => playerBaseState = value;
    }
}
