using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellManager : MonoBehaviour
{
    public abstract void CastSpell(int spellId, int spellServerId, int playerId, Vector3 spawnPoint, Vector3 moveDirection);

}
