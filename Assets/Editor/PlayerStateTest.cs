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

        IStateHelper statehelper = new StateHelper();

        Assert.AreEqual(playerState.GetType(), statehelper.GetTopState().GetType());
    }


    [Test]
    public void TestStatePushPop()
    {
        PlayerBaseState playerState = new MoveState();
        PlayerBaseState slideState = new SlideState(Vector3.forward, 20f, 3f);
        PlayerBaseState slideState2 = new SlideState(Vector3.forward, 40f, 4f);

        IStateHelper statehelper = new StateHelper();

        statehelper.AddState(slideState);
        statehelper.AddState(playerState);
        statehelper.AddState(slideState2);
        statehelper.RemoveState(slideState);

        Assert.AreEqual(playerState.GetType(), statehelper.GetTopState().GetType());
    }

}