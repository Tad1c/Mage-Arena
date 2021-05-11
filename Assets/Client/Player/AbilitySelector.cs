using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelector : MonoBehaviour
{

    public Toggle[] abilityToggles = new Toggle[4];
    // public AbilityManager abilityManager;

    public IntVariable selectedAbility;

    private void Start()
    {
        selectedAbility.Value = 0;
    }

    void Update()
    {
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
        if (abilityToggles.Length > index)
        {
            abilityToggles[index].isOn = true;
            // instead of toggle index, here we will need ability ID
            // and make sure player has bought this ability
            selectedAbility.Value = index; 
        }
    }

}
