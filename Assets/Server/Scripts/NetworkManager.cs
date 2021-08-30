using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using PlayFab.MultiplayerAgent;
using PlayFab;
using System.Linq;
using PlayFab.MultiplayerModels;

public class NetworkManager : MonoBehaviour
{

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;

        StartCoroutine(ReadyForPlayers());
    }

    private IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
        Debug.Log("Server ReadyForPlayers");
    }

    private void OnServerActive()
    {
        Debug.Log("Server active, OnServerActive");
        Invoke("StartShutdownLoop", 120);

        var ports = PlayFabMultiplayerAgentAPI.GetGameServerConnectionInfo().GamePortsConfiguration.ToList();
        var tcpPort = ports.Find(p => p.Name == "port_tcp");
        var udpPort = ports.Find(p => p.Name == "port_udp");

        var clientTcpPort = tcpPort.ClientConnectionPort;
        var serverTcpPort = tcpPort.ServerListeningPort;

        var clientUdpPort = udpPort.ClientConnectionPort;
        var serverUdpPort = udpPort.ServerListeningPort;

        foreach (PlayFab.MultiplayerAgent.Model.GamePort p in ports) {
            Debug.Log($"[Server] port name = {p.Name} , client = {p.ClientConnectionPort}, server = {p.ServerListeningPort}");
        }

        Server.Start(50, serverTcpPort, serverUdpPort);

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


}
