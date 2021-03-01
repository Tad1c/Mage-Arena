using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/StunAbility")]
public class StunAbility : Ability
{
    public float stunDuration = 1f;

    private Projectile _basicProjectile;

    public override void Initialize(Transform shootOrigin)
    {
        _basicProjectile = prefab;
        _shootOrigin = shootOrigin;

        if (_basicProjectile is StunProjectile projectile)
            projectile.stunDuration = stunDuration;
        
        _basicProjectile.range = range;
    }

    public override void FireAbility()
    {
       // _basicProjectile.Fire();
    }
}