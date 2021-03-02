using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : Projectile
{
    [HideInInspector] public float pushTime = 3f;
    [HideInInspector] public float pushForce = 50f;
    
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            OnHit(col);
        
        DeactiveProjectile();
    }

    private void OnHit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        Vector3 pushDirection = player.transform.position - transform.position; //player.transform.position - transform.position;
        //pushDirection = -pushDirection.normalized;

        player.TransitionToState(new SlideState(pushDirection, pushForce, pushTime));

        MyLog.D($"{player.username} was hit with push projectile and force is {pushForce}");
    }

    private void DeactiveProjectile()
    {
        this.gameObject.SetActive(false);
        ServerSend.DestroyBasicProjectile(this);
    }
}