using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEffectsOnPlayer
{
    void OnPush(Vector3 direction, float time, float force);
    void OnStun();
}
