using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpell : MonoBehaviour
{

    public Spell spell;

    protected static int nextProjectileId = 1;

    [HideInInspector] public int spellServerId;
    [HideInInspector] public int byPlayerId;
    [HideInInspector] public Vector3 finalPosition;
    [HideInInspector] public Vector3 castOrigin;

    public virtual void Init(Vector3 castOrigin, Vector3 shootTarget, int playerId)
    {
        byPlayerId = playerId;
        this.castOrigin = castOrigin;

        shootTarget = new Vector3(shootTarget.x, castOrigin.y, shootTarget.z);

        if (spell.spellRangeType == Spell.SpellRangeType.Dynamic)
        {
            float distance = Vector3.Distance(transform.position, shootTarget);
            if (distance > spell.maxRange)
            {
                Vector3 vect = transform.position - shootTarget;
                vect = vect.normalized;
                vect *= (distance - spell.maxRange);
                shootTarget += vect;

            }

            finalPosition = shootTarget;
        }
        else
        {
            finalPosition = transform.TransformPoint((shootTarget - castOrigin).normalized * spell.maxRange);
            Debug.Log("final spell pos: " + finalPosition);
        }

        transform.LookAt(finalPosition);
    }

    public virtual void InitOnClient(Vector3 shootTarget)
    {
        ServerSend.InitSpellOnClient(spell.id, spellServerId, byPlayerId, transform.position, shootTarget);
    }

    public virtual void DestroySpellClient(Vector3 pos)
    {
        transform.position = pos;
        Destroy(gameObject);
    }


    public void RequireCorrectSpellType<T>() where T : Spell
    {
        if (spell is T) return;
        throw new System.Exception(spell.name + " doesn't have correct Spell Type (Pushing, Stunning, etc..)");
    }

}
