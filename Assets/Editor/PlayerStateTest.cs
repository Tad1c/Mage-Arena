using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerStateTest
{

    [Test]
    public void TestStateInit()
    {
        PlayerBaseState playerState = new MoveState();

        IStateHelper state = new MockState()
        {
            playerState = playerState
        };

        StateHelper statehelper = new StateHelper(state);

        Assert.AreEqual(playerState.GetType(), statehelper.GetTopState().GetType());
    }


    [Test]
    public void TestStatePushPop()
    {
        PlayerBaseState playerState = new MoveState();
        PlayerBaseState slideState = new SlideState(Vector3.forward, 20f, 3f);
        PlayerBaseState slideState2 = new SlideState(Vector3.forward, 40f, 4f);

        IStateHelper state = new MockState()
        {
            playerState = playerState
        };

        StateHelper statehelper = new StateHelper(state);
        statehelper.PushState(slideState);
        statehelper.PushState(playerState);
        statehelper.PushState(slideState2);
        statehelper.PopState(slideState);

        Assert.AreEqual(playerState.GetType(), statehelper.GetTopState().GetType());
    }

}