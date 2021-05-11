﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static Dictionary<int, PlayerClient> players = new Dictionary<int, PlayerClient>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public List<GameObject> projectilePrefab;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation, float health)
    {
        GameObject player;

        if (id == Client.instance.myId)
            player = Instantiate(localPlayerPrefab, position, rotation);
        else
            player = Instantiate(playerPrefab, position, rotation);

        player.GetComponent<PlayerClient>().Init(id, username, health);


        players.Add(id, player.GetComponent<PlayerClient>());
    }

    public void SpawnProjectile(int id, int playerId, Vector3 position, Vector3 finalPosition, int type)
    {
        if (projectilePrefab.Count > type) {
            GameObject projectile = Instantiate(projectilePrefab[type], position, Quaternion.identity);

            ProjectileManager projectileManager = projectile.GetComponent<ProjectileManager>();
            projectiles.Add(id, projectileManager);
            projectileManager.Init(id, playerId, finalPosition);

            players[playerId].animator.SetTrigger("attack");
        }

    }

}
