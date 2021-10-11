using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    List<Player> playersOutsideZone = new List<Player>();

    CapsuleCollider c;

    private void Start()
    {
        c = GetComponent<CapsuleCollider>();
        StartCoroutine(DamageOverTime());
        StartCoroutine(SendZoneRadiusUpdates());
    }

    private void Update()
    {
        if (c.radius > 20f)
            c.radius -= 0.0007f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone");

            Player player = other.GetComponent<Player>();
            playersOutsideZone.Remove(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited zone");

            Player player = other.GetComponent<Player>();
            playersOutsideZone.Add(player);
        }
    }

    IEnumerator DamageOverTime()
    {
        foreach (Player p in playersOutsideZone)
        {
            p.HealthManager.TakeDamage(5);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DamageOverTime());
    }

    IEnumerator SendZoneRadiusUpdates()
    {
        ServerSend.ZoneRadius(c.radius);
        yield return new WaitForSeconds(2f);
        StartCoroutine(SendZoneRadiusUpdates());
    }
}
