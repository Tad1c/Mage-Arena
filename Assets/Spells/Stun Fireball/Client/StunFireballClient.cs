using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunFireballClient : SpellMovementStraight
{

    private StunningSpell _spell;

    public override void Init(Vector3 castOrigin, Vector3 dir, int playerId)
    {
        base.Init(castOrigin, dir, playerId);
        RequireCorrectSpellType<StunningSpell>();
        _spell = (StunningSpell)spell;
    }

}
