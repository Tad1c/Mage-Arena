using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLocal : SpellMovementStraight
{

    public override void Init(Vector3 castOrigin, Vector3 dir, int playerId)
    {
        base.Init(castOrigin, dir, playerId);
        RequireCorrectSpellType<PushingSpell>();
    }

}
