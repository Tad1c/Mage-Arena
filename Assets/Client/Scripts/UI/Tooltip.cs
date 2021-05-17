using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    private SpellUI spellItem;

    public TextMeshProUGUI textField;

    private void OnEnable()
    {
        Spell spell = spellItem.spell;
        var name = spell.name;

        var stats = $"Damage: {spell.damage}\nCooldown: {spell.cooldown}\n";

        // add logic to attributes of other types of spells
        if (spell is PushingSpell pushingSpell)
        {
            stats += $"Push force: {pushingSpell.pushForce}\n";
            stats += $"Push time: {pushingSpell.pushTime}\n";
        }

        textField.text = $"{name}\n\n{stats}\n{spell.description}";
    }

    public void SetSpellInfo(SpellUI hoveredSpell)
    {
        spellItem = hoveredSpell;
    }


}
