using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell List", menuName = "Spell List")]
public class SpellList : ScriptableObject
{
    public List<Spell> allSpells;
}
