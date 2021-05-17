using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticFireballLocal : ParabolicVerticalSpellMovement
{

    public List<GameObject> explosionPrefabs;

    public override void Init(Vector3 castOrigin, Vector3 shootTarget, int playerId)
    {
        base.Init(castOrigin, shootTarget, playerId);

        StartMovement();
    }

    public override void DestroySpellClient(Vector3 pos)
    {
        Instantiate(explosionPrefabs[0], transform.position, Quaternion.identity);
        base.DestroySpellClient(pos);
    }

}
