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

    public IntVariable selectedSpellId;

    private int selectedIndex = -1;

    public void StartCooldownForSelectedSpell()
    {
        if (selectedIndex >= 0 && selectedIndex < spells.Count) spells[selectedIndex].StartCooldown();
    }

    public void StartCooldownForSpellWithPosition(int spellPosition)
    {
        if (spellPosition >= 0 && spellPosition < spells.Count) spells[spellPosition].StartCooldown();
    }

    public int GetSpellIdAtPos(int pos)
    {
        if (pos < spells.Count)
        {
            return spells[pos].spell.id;
        }
        else
        {
            return -1;
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

    /*
      void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            toggleGroup.SetAllTogglesOff();
            selectedSpellId.Value = -1;
            selectedIndex = -1;
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
        if (selectedIndex == index) return;

        selectedIndex = index;

        if (spellToggles.Length > index)
        {
            var spellToggle = spellToggles[index];
            spellToggle.isOn = !spellToggle.isOn;

            if (spellToggle.isOn)
            {
                selectedSpellId.Value = spells[index].spell.id;
            }
            else
            {
                selectedSpellId.Value = -1;
            }
        }
    }
     */

}
