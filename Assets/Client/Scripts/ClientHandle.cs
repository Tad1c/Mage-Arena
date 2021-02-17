using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using ClientSide;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int id = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.instance.myId = id;

        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }


    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();

        Vector3 position = packet.ReadVector3();
        Quaternion rotation = packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(id, username, position, rotation);
    }

    public static void PlayerPosition(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();

        GameManager.players[id].transform.position = position;
    }

    public static void PlayerRotation(Packet packet)
    {
        int id = packet.ReadInt();
        Quaternion rotation = packet.ReadQuaternion();

        GameManager.players[id].transform.rotation = rotation;
    }

    public static void PlayerDisconnected(Packet pack)
    {
        int id = pack.ReadInt();

        Destroy(GameManager.players[id].gameObject);
        GameManager.players.Remove(id);
    }

    public static void PlayerHealth(Packet pack)
    {
        int id = pack.ReadInt();
        float health = pack.ReadFloat();

        GameManager.players[id].SetHealht(health);
    }

    public static void PlayerRespawn(Packet packet)
    {
        int id = packet.ReadInt();

        GameManager.players[id].Respawn();
    }

    public static void ProjectilePosition(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 finalPosition = packet.ReadVector3();

        //GameManager.projectiles[id].UpdatePosition(finalPosition);
    }

    public static void ProjectileSpawn(Packet packet)
    {
        int id = packet.ReadInt();
        int byPlayerId = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        Vector3 finalPosition = packet.ReadVector3();

        GameManager.instance.SpawnProjectile(id, byPlayerId, position, finalPosition);
    }

    public static void ProjectileDestroy(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();

        GameManager.projectiles[id].DestoryProjectile(position);
        GameManager.projectiles.Remove(id);
    }

    public static void Ping(Packet packet)
    {
        Client.instance.ping = Math.Round((DateTime.UtcNow - Client.instance.pingSent).TotalMilliseconds, 0f);
    }

    // public static void UDPTest(Packet packet)
    // {
    //     string msg = packet.ReadString();

    //     Debug.Log($"Received packet via UDP. Contains message: {msg}");
    //     ClientSend.UDPTestReceived();
    // }
}
