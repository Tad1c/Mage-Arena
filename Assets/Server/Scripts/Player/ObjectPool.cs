using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectPool
{
    private int _numberOfObjects;
    private GameObject _prefab;
    
    private  List<GameObject> objectPool = new List<GameObject>();
    private Transform _parentObject;

    public ObjectPool(int numberOfObjects, GameObject prefab, Transform parentObject)
    {
        _numberOfObjects = numberOfObjects;
        _prefab = prefab;
        _parentObject = parentObject;
    }

    public void CreatePool()
    {
        for (int i = 0; i < _numberOfObjects; i++)
        {
            GameObject obj = MonoBehaviour.Instantiate(_prefab, _parentObject.position, _parentObject.rotation, _parentObject);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    [CanBeNull]
    public GameObject GetPooledObject(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!objectPool[i].activeInHierarchy)
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
