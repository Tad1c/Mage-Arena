using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PushAbility")]
public class PushAbility : Ability
{
    
    public float pushTime = 3f;
    public float pushForce = 50f;

    private Projectile _projectile;
    
    public override void Initialize(Transform shootOrigin)
    {
        _projectile = prefab;

        if (_projectile is PushProjectile projectile)
        {
            projectile.speed = speed;
            projectile.pushTime = pushTime;
            projectile.pushForce = pushForce;
            projectile.range = range;
        }
    }

    public override void FireAbility()
    {
        //   projectile.Fire();
    }
}
