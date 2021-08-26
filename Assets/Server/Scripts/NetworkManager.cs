using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using PlayFab.MultiplayerAgent;
using PlayFab;
using System.Linq;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;

        var ports = PlayFabMultiplayerAgentAPI.GetGameServerConnectionInfo().GamePortsConfiguration.ToList();
        var tcpp = ports.Find(p => p.Name == "port_tcp").ClientConnectionPort;
        var udpp = ports.Find(p => p.Name == "port_udp").ClientConnectionPort;
        Server.Start(50, tcpp, udpp);
    }

    private void OnServerActive()
    {
        Debug.Log("Server is ready for players.");
        Invoke("StartShutdownLoop", 120);
        // players can now connect to the server
    }

    void StartShutdownLoop()
    {
        StartCoroutine(ShutDownWhenAllDisconnected());
    }

    IEnumerator ShutDownWhenAllDisconnected()
    {
        var objs = GameObject.FindGameObjectsWithTag("Player");
        if (objs.Length <= 1)
        {
            Application.Quit();
        }
        yield return new WaitForSeconds(10f);
        StartCoroutine(ShutDownWhenAllDisconnected());
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstanciatePlayer()
    {
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }

}
