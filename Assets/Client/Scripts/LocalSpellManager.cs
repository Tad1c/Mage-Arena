using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SpellManager keeps reference to all spells in the game.
 * Also takes care of instantiating specific spell.
 */
public class LocalSpellManager : SpellManager
{
    public static LocalSpellManager instance;

    public static Dictionary<int, BaseSpell> initiatedSpells = new Dictionary<int, BaseSpell>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public override void CastSpell(int spellId, int spellServerId, int playerId, Vector3 spawnPoint, Vector3 shootTarget)
    {
        var spellToCast = SpellDatabase.instance.GetSpellById(spellId);
        Debug.Log(spellToCast);
        if (spellToCast != null)
        {
            var obj = Instantiate(spellToCast.localSpellPrefab, spawnPoint, Quaternion.identity);
            var spell = obj.GetComponent<BaseSpell>();
            spell.Init(spawnPoint, shootTarget, playerId);
            initiatedSpells[spellServerId] = spell;
        }
    }

    public void DestroySpell(int spellServerId, Vector3 position)
    {
        initiatedSpells[spellServerId].DestroySpellClient(position);
        initiatedSpells.Remove(spellServerId);
    }

}
