using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static Dictionary<int, PlayerClient> players = new Dictionary<int, PlayerClient>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

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

}
