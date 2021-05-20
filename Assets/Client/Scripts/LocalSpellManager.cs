using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalSpellManager : SpellManager
{
    public static LocalSpellManager instance;

    public static List<BaseSpell> initiatedSpells = new List<BaseSpell>();

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
            spell.spellServerId = spellServerId;
            initiatedSpells.Add(spell);
        }
    }

    public void DestroySpell(int spellServerId, Vector3 position)
    {
        var spell = initiatedSpells.Find(s => s.spellServerId == spellServerId);
        spell.DestroySpellClient(position);
        initiatedSpells.Remove(spell);
    }

}
