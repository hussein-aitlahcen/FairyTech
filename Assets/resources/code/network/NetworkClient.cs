using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FairyTech.Common.Generic;
using FairyTech.Common.Network.Protocol;
using FairyTech.Common.Network.Protocol.Type.Message;
using Lidgren.Network;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public sealed class NetworkClient
{
    private const int INITIAL_NET_CAPACITY = 100;
    private static readonly TimeSpan UPDATE_DELAY = TimeSpan.FromMilliseconds(10);

    private NetConnection m_connection;
    private readonly NetClient m_client;
    private readonly NetQueue<AbstractNetworkMessage> m_incommingMessages; 

    public NetworkClient()
    {
        m_incommingMessages = new NetQueue<AbstractNetworkMessage>(INITIAL_NET_CAPACITY);
        m_client = new NetClient(new NetPeerConfiguration("FairyTech"));
    }

    public NetConnectionStatus ConnectionStatus
    {
        get { return m_client.ConnectionStatus; }
    }

    public AbstractNetworkMessage NextMessage
    {
        get
        {
            AbstractNetworkMessage msg;
            m_incommingMessages.TryDequeue(out msg);
            return msg;
        }
    }

    public void Start()
    {
        m_client.Start();
        new Thread(Tick).Start();
    }

    public void Login(string userName, string password)
    {
        if (ConnectionStatus != NetConnectionStatus.Disconnected)
            return;

        m_connection = m_client.Connect("127.0.0.1", 2648);
        SendReliable(new CM_AuthenticationRequest
        {
            UserName = userName,
            Password = password
        });
    }

    private void Tick()
    {
        while (true)
        {
            NetIncomingMessage message;
            while ((message = m_client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                        break;

                    case NetIncomingMessageType.Data:
                        try
                        {
                            var deserialized = AbstractNetworkMessage.Deserialize(message.ReadBytes(message.ReadInt32()));
                            Debug.Log("message < " + deserialized.GetType().Name);
                            m_incommingMessages.Enqueue(deserialized);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                        Debug.LogWarning("message received");
                        break;
                }
            }
            Thread.Sleep(UPDATE_DELAY);
        }
    }

    public void SendReliable(AbstractNetworkMessage message)
    {
        var serialized = message.Serialize();
        var outgoing = m_connection.Peer.CreateMessage(serialized.Length);
        outgoing.Write(serialized.Length);
        outgoing.Write(serialized);
        m_connection.SendMessage(outgoing, NetDeliveryMethod.ReliableOrdered, 0);
    }
}
