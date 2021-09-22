using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovementStraight : BaseSpell
{

    public virtual void Update()
    {
        float step = spell.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);
    }

}
