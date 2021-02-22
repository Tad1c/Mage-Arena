using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHelper
{
    public void AddState(PlayerBaseState state);
    public void RemoveState(PlayerBaseState state);
    public PlayerBaseState GetTopState();
}
