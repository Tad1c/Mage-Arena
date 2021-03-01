using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectile : Projectile
{
    [HideInInspector] public float stunDuration = 1f;
    
    private Rigidbody _rb;

    private Vector3 finalDestination;

    public override void Init(Vector3 direction, int playerId)
    {
        byPlayerId = playerId;
        finalDestination = transform.TransformPoint(direction * range);
        transform.LookAt(finalDestination);
        
        id = nextProjectileId;
        nextProjectileId++;
        
        projectileDic.Add(id, this);

        ServerSend.InstantiateBasicProjectile(this, byPlayerId, finalDestination, type);
    }

    private void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, finalDestination, step);

        if (Vector3.Distance(transform.position, finalDestination) < 0.01f)
            DeactiveProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnHit(other);

            DeactiveProjectile();
    }

    private void OnHit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        
        player.HealthManager.TakeDamage(damage);
        player.TransitionToState(new StunState(stunDuration));
        
        MyLog.D($"{player.username} was hit with stun projectile and stun timer is {stunDuration}");
    }

    private void DeactiveProjectile()
    {
        this.gameObject.SetActive(false);
        ServerSend.DestroyBasicProjectile(this);
    }
}