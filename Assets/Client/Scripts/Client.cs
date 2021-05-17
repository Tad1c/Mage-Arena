using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

namespace ClientSide
{
    public class Client : MonoBehaviour
    {
        public static Client instance;

        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";
        public int port = 26950;
        public int myId = 0;
        public TCP tcp;
        public UDP udp;

        [HideInInspector]
        public DateTime pingSent;
        [HideInInspector]
        public double ping;

        private bool isConnected = false;

        private delegate void PacketHandler(Packet packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }


        private void OnApplicationQuit()
        {
            Disconnect();
        }

        public void ConnectToServer()
        {

            tcp = new TCP();
            udp = new UDP();

            InitializeClientData();

            isConnected = true;

            tcp.Connect();
        }

        public void SetIP(string ip)
        {
            this.ip = ip;
        }

        // public void SetPORT(string port)
        // {
        //     this.port = Int32.Parse(port);
        // }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);

                if (!socket.Connected)
                    return;

                stream = socket.GetStream();

                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to server via TCP: {e}");
                }

            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLenght = stream.EndRead(result);
                    if (byteLenght <= 0)
                    {
                        instance.Disconnect();
                        return;
                    }

                    byte[] data = new byte[byteLenght];
                    Array.Copy(receiveBuffer, data, byteLenght);

                    receivedData.Reset(HandledData(data));

                    //TODO: handle data
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
                }
            }

            private bool HandledData(byte[] data)
            {
                int packetLength = 0;

                receivedData.SetBytes(data);

                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();

                    if (packetLength <= 0)
                        return true;
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetLength);

                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            packetHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;

                    if (receivedData.UnreadLength() >= 4)
                    {
                        packetLength = receivedData.ReadInt();

                        if (packetLength <= 0)
                            return true;
                    }
                }

                if (packetLength <= 1)
                    return true;

                return false;
            }

            private void Disconnect()
            {
                instance.Disconnect();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }


        }


        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
            }

            public void Connect(int localPort)
            {
                socket = new UdpClient(localPort);

                socket.Connect(endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                using (Packet packet = new Packet())
                {
                    SendData(packet);
                }
            }

            public void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(instance.myId);
                    if (socket != null)
                    {
                        socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to server via UDP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    byte[] data = socket.EndReceive(result, ref endPoint);
                    socket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4)
                    {
                        instance.Disconnect();
                        return;
                    }

                    HandleData(data);

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message ?? "Client ReceiveCallback Error");
                    Disconnect();
                }
            }

            private void Disconnect()
            {
                instance.Disconnect();

                endPoint = null;
                socket = null;
            }

            private void HandleData(byte[] data)
            {
                using (Packet packet = new Packet(data))
                {
                    int packetLength = packet.ReadInt();
                    data = packet.ReadBytes(packetLength);
                }

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(data))
                    {
                        int packetId = packet.ReadInt();
                        packetHandlers[packetId](packet);
                    }
                });
            }
        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.welcome, ClientHandle.Welcome},
                {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
                {(int)ServerPackets.playerPosition, ClientHandle.PlayerPosition},
                {(int)ServerPackets.playerRotation, ClientHandle.PlayerRotation},
                {(int)ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected},
                {(int)ServerPackets.playerHealth, ClientHandle.PlayerHealth},
                {(int)ServerPackets.playerRespawn, ClientHandle.PlayerRespawn},
                {(int)ServerPackets.projectileShoot, ClientHandle.ProjectileSpawn},
                {(int)ServerPackets.projectilePosition, ClientHandle.ProjectilePosition},
                {(int)ServerPackets.projectileDestroy, ClientHandle.DestorySpellOnClient},
                {(int)ServerPackets.playerJump, ClientHandle.Jump},
                {(int)ServerPackets.ping, ClientHandle.Ping},
                // {(int)ServerPackets.udpTest, ClientHandle.UDPTest}
            };

            Debug.Log("Initialize client data");
        }


        private void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                tcp.socket.Close();
                udp.socket.Close();

                Debug.Log("Disconnected from server");
            }
        }
    }
}