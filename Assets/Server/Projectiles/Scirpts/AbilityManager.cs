using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<Ability> _abilities = new List<Ability>();

    public GameObject prefab;

    public Transform shootOrigin;

    public Transform parentObject;

    private ObjectPool _objectPool;

    public void Start()
    {
        _objectPool = new ObjectPool(10, prefab, parentObject);
        _objectPool.CreatePool();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Shoot();
    }

    public void Shoot()
    {
        GameObject projectile = _objectPool.GetPooledObject(1);

        if (projectile != null)
        {
            var basicPro = projectile.GetComponent<BasicProjectile>();
            var position = shootOrigin.position;

            projectile.transform.localPosition = position;

            basicPro.Init(shootOrigin.forward, 2);
            projectile.SetActive(true);
        }
    }

    public void BuyAbility(Ability ability)
    {
        ability.Initialize(shootOrigin);

        _abilities.Add(ability);
    }

    public Ability SelectAbility(int abilityNumber)
    {
        if (_abilities.Count <= abilityNumber)
            return null;

        return _abilities[abilityNumber];
    }
}