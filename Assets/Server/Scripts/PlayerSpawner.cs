using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;

    public static PlayerSpawner instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public Player InstanciatePlayer()
    {
        var randomOffset = Random.Range(0.5f, 2f);
        var spawnPosition = Vector3.zero + Vector3.up * randomOffset;
        Debug.Log($"[Server] Spawning player prefab at: {spawnPosition}");
        return Instantiate(playerPrefab, spawnPosition, Quaternion.identity).GetComponent<Player>();
    }

}
