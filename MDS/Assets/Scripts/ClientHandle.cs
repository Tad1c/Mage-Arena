using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
public class ClientHandle
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

    public static void UDPTest(Packet packet)
    {
        string msg = packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {msg}");
        ClientSend.UDPTestReceived();
    }
}
