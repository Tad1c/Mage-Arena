using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectPool
{
    private int _numberOfObjects;
    private Projectile _prefab;
    
    private  List<Projectile> objectPool = new List<Projectile>();
    private Transform _parentObject;

    public ObjectPool(int numberOfObjects, Projectile prefab, Transform parentObject)
    {
        _numberOfObjects = numberOfObjects;
        _prefab = prefab;
        _parentObject = parentObject;
    }

    public void CreatePool()
    {
        for (int i = 0; i < _numberOfObjects; i++)
        {
            Projectile obj = MonoBehaviour.Instantiate(_prefab, _parentObject.position, _parentObject.rotation, _parentObject);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }
    }

    [CanBeNull]
    public Projectile GetPooledObject(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!objectPool[i].gameObject.activeInHierarchy)
                return objectPool[i];
            else 
            {
                if(objectPool.Count <= amount)
                    continue;
                
                amount++;
            }
        }
        return null;
    }

}
