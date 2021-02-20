using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelathManager : MonoBehaviour
{

    private float _health;

    public float Health
    {
        get => _health;
    }

    public float maxHealth = 100f;



    public void TakeDmg(float dmg)
    {
        if(_health <= 0)
            return;

        _health -= dmg;

        if (_health <= 0)
        {
            _health = 0;
            
        }
    }
    
    
    private void OnEnable()
    {
        PlayerManager.OnSetId += SetId;
    }

    public void SetId(int id)
    {
        Debug.Log(id);
    }
}
