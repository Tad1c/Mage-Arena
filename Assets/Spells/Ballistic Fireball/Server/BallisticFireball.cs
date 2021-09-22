using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticFireball : ParabolicVerticalSpellMovement
{

    private PushingSpell _spell;

    public float explosionRadius = 20f;

    public override void Init(Vector3 castOrigin, Vector3 shootTarget, int playerId)
    {
        base.Init(castOrigin, shootTarget, playerId);

        RequireCorrectSpellType<PushingSpell>();
        _spell = (PushingSpell)spell;

        spellServerId = nextProjectileId;
        nextProjectileId++;

        base.InitOnClient(shootTarget);

        StartMovement();
    }

    public override void OnMovementFinished()
    {
        ExplosionDamage(transform.position, explosionRadius);
        Destroy();
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Player hit by explosion");

                Player player = hitCollider.GetComponent<Player>();
                Vector3 pushDirection = player.transform.position - transform.position;
                player.HealthManager.TakeDamage(_spell.damage);
                player.TransitionToState(new SlideState(pushDirection, _spell.pushForce, _spell.pushTime));
            }
        }

    }

    private void Destroy()
    {
        ServerSend.DestorySpellOnClient(spellServerId, transform.position);
        Destroy(gameObject);
    }

}
