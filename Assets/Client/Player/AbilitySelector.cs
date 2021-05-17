using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelector : MonoBehaviour
{

    public ToggleGroup toggleGroup;

    public Toggle[] spellToggles = new Toggle[4];

    [HideInInspector]
    public List<Spell> spells = new List<Spell>();

    public IntVariable selectedSpellId;

    private void Start()
    {
        for (int i = 0; i < spellToggles.Length; i++)
        {
            if (spellToggles[i].TryGetComponent<SpellUI>(out var spellUI))
            {
                spells[i] = spellUI.spell;
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            toggleGroup.SetAllTogglesOff();
            selectedSpellId.Value = -1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandleAbilitySelect(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandleAbilitySelect(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandleAbilitySelect(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HandleAbilitySelect(3);
        }
    }

    private void HandleAbilitySelect(int index)
    {
        if (spellToggles.Length > index)
        {
            var spellToggle = spellToggles[index];
            spellToggle.isOn = !spellToggle.isOn;

            if (spellToggle.isOn)
            {
                selectedSpellId.Value = spells[index].id;
            }
            else
            {
                selectedSpellId.Value = -1;
            }
        }
    }

}
