using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelector : MonoBehaviour
{
    public static AbilitySelector instance;

    public ToggleGroup toggleGroup;

    public Toggle[] spellToggles = new Toggle[4];

    [HideInInspector]
    public List<SpellUI> spells = new List<SpellUI>();

    public SpellUI GetSpellAtPosition(int pos) {
        if (pos < spells.Count)
        {
            return spells[pos];
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }


    private void Start()
    {
        for (int i = 0; i < spellToggles.Length; i++)
        {
            if (spellToggles[i].TryGetComponent<SpellUI>(out var spellUI))
            {
                spells[i] = spellUI;
            }
        }
    }

}
