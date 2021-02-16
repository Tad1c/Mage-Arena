using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{


    public int id;
    public string username;

    public TextMeshProUGUI user_text;
    public Slider healthSlider;

    public float health;
    public float maxHealth;
    public MeshRenderer model;

    public void Init(int id, string userName)
    {
        this.id = id;
        this.username = userName;
        health = maxHealth;

        user_text.text = username;
    }

    public void SetHealht(float health)
    {
        this.health = health;
        healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealht(maxHealth);
    }

}
