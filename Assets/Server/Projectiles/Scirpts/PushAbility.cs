using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PushAbility")]
public class PushAbility : Ability
{
    
    public float pushTime = 3f;
    public float pushForce = 50f;

    private BasicProjectile _projectile;
    
    public override void Initialize(Transform shootOrigin)
    {
        _projectile = prefab.GetComponent<BasicProjectile>();
        _projectile.damage = damage;
        _projectile.speed = speed;
        _projectile.pushTime = pushTime;
        _projectile.pushForce = pushForce;
        _projectile.range = range;
    }

    public override void FireAbility()
    {
        _projectile.Fire();
    }
}
