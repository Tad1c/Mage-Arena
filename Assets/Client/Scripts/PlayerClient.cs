using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ClientSide;

public class PlayerClient : MonoBehaviour
{

    public int id;
    public string username;

    public TextMeshProUGUI user_text;
    public TextMeshProUGUI pingText;
    public Slider healthSlider;

    public float health;
    public float maxHealth;
    public MeshRenderer model;

    public float pingCountdown = 1f;
    public float pingCountDownLimit = 1f;

    public Animator animator;
    private void FixedUpdate()
    {
        pingCountdown += Time.fixedDeltaTime;
        if (pingCountdown >= pingCountDownLimit)
        {
            pingCountdown = 0;
            pingText.text = Client.instance.ping.ToString();
            ClientSend.Ping();
        }
    }

    public void Init(int id, string userName, float health)
    {
        this.id = id;
        this.username = userName;
        SetHealth(health);

        user_text.text = username;
    }

    public void SetHealth(float health)
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
        SetHealth(maxHealth);
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }


    public void InterpolateMovement(Vector3 newPosition)
    {
        this.transform.position = newPosition;
        //StartCoroutine(MoveObject(transform.position, newPosition, 0.05f));
    }

    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
    }

}
