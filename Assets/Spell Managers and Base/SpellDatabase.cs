using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellDatabase : MonoBehaviour
{

    public SpellList spells;

    public static SpellDatabase instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public Spell GetSpellById(int spellId) {
        return instance.spells.allSpells.Find(spell => spell.id == spellId);
    }

}
