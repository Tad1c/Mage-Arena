using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SpellManager keeps reference to all spells in the game.
 * Also takes care of instantiating specific spell.
 */
public class ServerSpellManager : SpellManager
{
    public static ServerSpellManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public override void CastSpell(int spellId, int _, int playerId, Vector3 spawnPoint, Vector3 shootTarget)
    {
        // check if playerId owns spellId
        var spellToCast = SpellDatabase.instance.GetSpellById(spellId);
        if (spellToCast != null)
        {
            var obj = Instantiate(spellToCast.spellPrefab, spawnPoint, Quaternion.identity);
            var spell = obj.GetComponent<BaseSpell>();
            spell.Init(spawnPoint, shootTarget, playerId);
        }
    }

}
