using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    public int damage = 10;
    public float speed = 30f;
    public float range = 30f;
    [HideInInspector] public float cooldown = 0f;
    public float fireRate = 10;
    public Projectile prefab;

    protected Transform _shootOrigin;

    private void OnEnable()
    {
        cooldown = 0f;
    }

    public abstract void Initialize(Transform shootOrigin);
    public abstract void FireAbility();
}