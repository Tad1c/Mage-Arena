using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;
using System;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.udp.SendData(packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(Client.instance.myId);
            packet.Write(CurrentPlayerData.playerDisplayName);

            SendTCPData(packet);
        }
    }

    public static void PlayerMovement(float[] inputs)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerMovement))
        {
            packet.Write(inputs.Length);
            foreach (var input in inputs)
            {
                packet.Write(input);
            }

            packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(packet);
        }
    }

    public static void PlayerJump()
    {
        using (Packet packet = new Packet((int)ClientPackets.playerJump))
        {
            packet.Write(true);

            SendTCPData(packet);
        }
    }

    public static void ShootProjectile(Vector3 shootDirection, int spellPos)
    {
        using (Packet packet = new Packet((int)ClientPackets.shootProjectile))
        {
            packet.Write(shootDirection);
            packet.Write(spellPos);
            SendTCPData(packet);
        }
    }

    public static void Ping()
    {
        Client.instance.pingSent = DateTime.UtcNow;
        using (Packet packet = new Packet((int)ClientPackets.ping))
        {
            SendTCPData(packet);
        }
    }

    // public static void UDPTestReceived()
    // {
    //     using (Packet packet = new Packet((int)ClientPackets.udpTestReceive))
    //     {
    //         packet.Write("Received a UDP packet.");

    //         SendUDPData(packet);
    //     }
    // }
    #endregion
}
