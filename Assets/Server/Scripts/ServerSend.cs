﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ServerSend
{

    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();

        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();

        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
                Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
                Server.clients[i].udp.SendData(packet);
        }
    }

    public static void Welcome(int toClient, string msg)
    {
        Packet packet = new Packet((int)ServerPackets.welcome);

        packet.Write(msg);
        packet.Write(toClient);

        SendTCPData(toClient, packet);
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.HealthManager.Health);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(player.id);
            packet.Write(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            packet.Write(player.transform.position);
            packet.Write(player.Controller.velocity.magnitude);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerRotation(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerRotation))
        {
            packet.Write(player.id);
            packet.Write(player.transform.rotation);

            SendUDPDataToAll(player.id, packet);
        }
    }

    public static void Jump(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerJump))
        {
            // the invocation of this method itself means the player jumped
            packet.Write(player.id);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            packet.Write(playerId);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(int id, float health)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerHealth))
        {
            packet.Write(id);
            //TODO: Uncomment this line of code.
            packet.Write(health);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRespawn(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerRespawn))
        {
            packet.Write(player.id);

            SendTCPDataToAll(packet);
        }
    }

    #region Spell functions

    public static void InitSpellOnClient(int spellId, int spellServerId, int playerId, Vector3 startPosition, Vector3 shootTarget)
    {
        using Packet packet = new Packet((int)ServerPackets.projectileShoot);

        packet.Write(spellId);
        packet.Write(spellServerId);
        packet.Write(playerId);
        packet.Write(startPosition);
        packet.Write(shootTarget);

        SendTCPDataToAll(packet);
    }

    public static void DestorySpellOnClient(int spellServerId, Vector3 finalPosition)
    {
        using (Packet packet = new Packet((int)ServerPackets.projectileDestroy))
        {
            packet.Write(spellServerId);
            packet.Write(finalPosition);

            SendTCPDataToAll(packet);
        }
    }

    public static void ProjectilePosition(int id, Vector3 dest)
    {
        using (Packet packet = new Packet((int)ServerPackets.projectilePosition))
        {
            packet.Write(id);
            packet.Write(dest);

            SendTCPDataToAll(packet);
        }
    }
    #endregion

    public static void Ping(int id)
    {
        using (Packet packet = new Packet((int)ServerPackets.ping))
        {
            SendTCPData(id, packet);
        }
    }




    //public static void UDPTest(int toClient)
    //{
    //    using (Packet packet = new Packet((int)ServerPackets.udpTest))
    //    {
    //        packet.Write("A test pacekt from UDP");

    //        SendUDPData(toClient, packet);
    //    }
    //}
}

