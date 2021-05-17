using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{

    public int id;
    public new string name = "New Spell";
    public string description = "Spell Description";

    public Sprite icon;

    public int damage = 10;
    public float speed = 30f;
    public float maxRange = 30f;
    public float cooldown = 3;


    //This is what you need to show in the inspector.
    public SpellRangeType spellRangeType;

    public GameObject spellPrefab;
    public GameObject localSpellPrefab;


    public enum SpellRangeType { Fixed, Dynamic};


}