using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject projectilePrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
    {
        GameObject player;

        if (id == Client.instance.myId)
            player = Instantiate(localPlayerPrefab, position, rotation);
        else
            player = Instantiate(playerPrefab, position, rotation);

        player.GetComponent<PlayerManager>().Init(id, username);


        players.Add(id, player.GetComponent<PlayerManager>());
    }

    public void SpawnProjectile(int id, int playerId, Vector3 position, Vector3 finalPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);

        ProjectileManager projectileManager = projectile.GetComponent<ProjectileManager>();
        projectiles.Add(id, projectileManager);
        projectileManager.Init(id, playerId, finalPosition);

    }

}
