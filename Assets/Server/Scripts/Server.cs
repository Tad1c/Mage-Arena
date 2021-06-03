using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using ServerSide;

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int TCPPort { get; private set; }
    public static int UDPPort { get; private set; }

    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int maxPlayers, int tcpPort, int udpPort)
    {
        MaxPlayers = maxPlayers;

        TCPPort = tcpPort;
        UDPPort = udpPort;

        MyLog.D("Starting server...");
        InitializeServerData();

        IPAddress ipAddress = IPAddress.Any;

        tcpListener = new TcpListener(ipAddress, TCPPort);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(UDPPort);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        MyLog.D($"Server started on {ipAddress}:{TCPPort}");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }

    private static void UDPConnectCallback(IAsyncResult result)
    {

    }

    private static void UDPReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(result, ref clientEndPoint);

            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet packet = new Packet(data))
            {
                int clientId = packet.ReadInt();

                if (clientId == 0)
                    return;

                if (clients[clientId].udp.endPoint == null)
                {
                    clients[clientId].udp.Connect(clientEndPoint);
                    return;
                }

                if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientId].udp.HandleData(packet);
                }
            }
        }
        catch (Exception e)
        {
            MyLog.D($"Error receiving UDP data: {e}");
        }
    }

    public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            if (clientEndPoint != null)
            {
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
        }
        catch (Exception e)
        {
            MyLog.D($"Error sending data to {clientEndPoint} via UDP {e}");
        }
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        //String the tcp client instance in local variable
        TcpClient client = tcpListener.EndAcceptTcpClient(result);

        //Once client is connected we need to make sure that he is continuing listening for connections
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        MyLog.D($"Incoming connect from {client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(client);
                return;
            }
        }

        MyLog.D($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
            {

                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived},
                {(int)ClientPackets.playerMovement, ServerHandle.PlayerMovement},
                {(int)ClientPackets.shootProjectile, ServerHandle.PlayerShootProjectile},
                {(int)ClientPackets.playerJump, ServerHandle.PlayerJump},
                {(int)ClientPackets.ping, ServerHandle.Ping}

               // {(int)ClientPackets.udpTestReceive, ServerHandle.UDPTestReceived}
            };
        MyLog.D("Initialized packets");
    }
}
