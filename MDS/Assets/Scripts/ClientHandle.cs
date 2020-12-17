using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int id = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.instance.myId = id;

        ClientSend.WelcomeReceived();
        //TODO: send welcome receivde
    }
}
