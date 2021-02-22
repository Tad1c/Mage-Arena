using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthManager : MonoBehaviour, IHealth
{
    private float _currentHealth;

    private HealthController _healthController;

    private float _maxHealth = 100;

    private Player _player;

    public float Health
    {
        get => _currentHealth;
        set {
            _currentHealth = value;
            if (value <= 0) _healthController.PlayerDied();
        }
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _healthController = new HealthController(this, _maxHealth);
    }

    public void TakeDamage(float amount)
    {
        _healthController.TakeDmg(amount);
        ServerSend.PlayerHealth(_player.id, Health);
    }

    public void Died()
    {
        //Disable movement
        //Send event to playerManager for respawn
    }

    public void Respawn()
    {
        _healthController.RefillHealth(_maxHealth);
        ServerSend.PlayerHealth(_player.id, _maxHealth);
    }
    
}