using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{

    public string abilityName = "New Ability";
    public int damage = 10;
    public float speed = 30f;
    public float range = 30f;
    public GameObject prefab;
    
    protected Transform _shootOrigin;
    
    public abstract void Initialize(Transform shootOrigin);
    public abstract void FireAbility();

}
