using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHelper
{
    int StatesCount();
    void AddState(PlayerBaseState state);
    void RemoveState(PlayerBaseState state);
    PlayerBaseState GetTopState();
}
