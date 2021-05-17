using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Pushing Spell")]
public class PushingSpell : Spell
{
    public float pushTime = 3f;
    public float pushForce = 50f;
}
