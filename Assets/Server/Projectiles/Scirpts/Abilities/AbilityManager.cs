using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<Ability> abilitiesToBuy =  new List<Ability>(); 
    [SerializeField] private List<Ability> currentAbilities = new List<Ability>();

    //   public GameObject prefab;

    public Transform shootOrigin;

    public Transform parentObject;

    private List<ObjectPool> _objectPool = new List<ObjectPool>();

    private int objetPoolCount = 0;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void Start()
    {
        for (int i = 0; i < currentAbilities.Count; i++)
        {
            currentAbilities[i].Initialize(shootOrigin);
            _objectPool.Add(ObjectPool(i));
        }
    }

    private ObjectPool ObjectPool(int i)
    {
        var obj = new ObjectPool(3, currentAbilities[i].prefab, parentObject);
        obj.CreatePool();
        objetPoolCount++;
        return obj;
    }

    //Only for testing purposes
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Shoot(shootOrigin.forward,0);
        if (Input.GetKeyDown(KeyCode.Y))
            Shoot(shootOrigin.forward,1);
        if (Input.GetKeyDown(KeyCode.U))
            Shoot(shootOrigin.forward,2);

        if (Input.GetKeyDown(KeyCode.P))
        {
            BuyAbility(abilitiesToBuy[0]);
        }
    }

    public void Shoot(Vector3 origin, int index, int amount = 1)
    {
        if(_player.StateHelper.HasState<StunState>())
            return;

        if (Time.time >= currentAbilities[index].cooldown)
        {
            currentAbilities[index].cooldown = Time.time + 1f/ currentAbilities[index].fireRate;
            
            var projectile = _objectPool[index].GetPooledObject(amount);

            if (projectile != null)
            {
                projectile.transform.localPosition = shootOrigin.position;
                projectile.Init(origin, _player.id);
                projectile.gameObject.SetActive(true);
            //    ServerSend.InstantiateBasicProjectile(projectile, _player.id, origin, index);
            }
        }
    }

    public void BuyAbility(Ability ability)
    {
        ability.Initialize(shootOrigin);
        this.currentAbilities.Add(ability);
        _objectPool.Add(ObjectPool(objetPoolCount));
    }

    public Ability SelectAbility(int abilityNumber)
    {
        if (currentAbilities.Count <= abilityNumber)
            return null;

        return currentAbilities[abilityNumber];
    }
}