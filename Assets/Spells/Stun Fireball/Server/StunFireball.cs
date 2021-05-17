using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunFireball : SpellMovementStraight
{

    private StunningSpell _spell;

    public override void Init(Vector3 castOrigin, Vector3 shootTarget, int playerId)
    {
        base.Init(castOrigin, shootTarget, playerId);

        RequireCorrectSpellType<StunningSpell>();
        _spell = (StunningSpell)spell;

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

        player.TransitionToState(new StunState(_spell.stunDuration));

        MyLog.D($"{player.username} was hit with stun projectile");
    }

    private void Destroy()
    {
        ServerSend.DestorySpellOnClient(spellServerId, transform.position);
        Destroy(gameObject);
    }
}