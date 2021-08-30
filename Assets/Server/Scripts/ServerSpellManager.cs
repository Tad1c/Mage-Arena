using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ServerSpellManager : SpellManager
{
    public static ServerSpellManager instance;

    // playerid / list of spellid
    public static Dictionary<int, List<int>> spellCooldowns = new Dictionary<int, List<int>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public override void CastSpell(int spellId, int _, int playerId, Vector3 spawnPoint, Vector3 shootTarget)
    {
        // check if playerId has casted spellId previously
        // if it's in the dictionary, it means cooldown time hasen't expired, so don't cast this spell
        if (spellCooldowns.ContainsKey(playerId))
        {
            foreach (int sId in spellCooldowns[playerId])
            {
                if (sId == spellId)
                {
                    return;
                }
            }
        }
        else
        {
            spellCooldowns[playerId] = new List<int>();
        }

        // check if playerId owns spellId
        var spellToCast = SpellDatabase.instance.GetSpellById(spellId);
        if (spellToCast != null)
        {
            var obj = Instantiate(spellToCast.spellPrefab, spawnPoint, Quaternion.identity);
            var baseSpell = obj.GetComponent<BaseSpell>();
            baseSpell.Init(spawnPoint, shootTarget, playerId);
            spellCooldowns[playerId].Add(spellId);
            StartCoroutine(CooldownCoroutine(playerId, baseSpell.spell));
        }
    }

    private IEnumerator CooldownCoroutine(int playerId, Spell spell)
    {
        yield return new WaitForSeconds(spell.cooldown);
        spellCooldowns[playerId].Remove(spell.id);

    }

}