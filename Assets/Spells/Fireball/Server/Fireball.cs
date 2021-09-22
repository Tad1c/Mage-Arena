using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellMovementStraight
{

    private PushingSpell _spell;

    public override void Init(Vector3 castOrigin, Vector3 shootTarget, int playerId)
    {
        base.Init(castOrigin, shootTarget, playerId);

        RequireCorrectSpellType<PushingSpell>();
        _spell = (PushingSpell)spell;

        spellServerId = nextProjectileId;
        nextProjectileId++;

        base.InitOnClient(shootTarget);
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, finalPosition) < 0.01f)
            Destroy();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            OnHit(col);

        Destroy();
    }

    private void OnHit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        Vector3 pushDirection = player.transform.position - transform.position;

        player.HealthManager.TakeDamage(_spell.damage);

        player.TransitionToState(new SlideState(pushDirection, _spell.pushForce, _spell.pushTime));

        MyLog.D($"{player.username} was hit with push projectile and force is {_spell.pushForce}");
    }

    private void Destroy()
    {
        ServerSend.DestorySpellOnClient(spellServerId, transform.position);
        Destroy(gameObject);
    }
}