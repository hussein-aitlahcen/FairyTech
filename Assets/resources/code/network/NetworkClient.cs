using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FairyTech.Common.Generic;
using FairyTech.Common.Network.Protocol;
using FairyTech.Common.Network.Protocol.Type.Message;
using Lidgren.Network;
using UnityEngine;

public sealed class NetworkClient
{
    private static TimeSpan UPDATE_DELAY = TimeSpan.FromMilliseconds(10);

    private NetClient m_client;

    public NetworkClient()
    {
        m_client = new NetClient(new NetPeerConfiguration("FairyTech"));
    }

    public void Start()
    {
        m_client.Start();
        m_client.Connect("127.0.0.1", 2648);
        new Thread(Tick).Start();
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
                            new TypeSwitch(deserialized)
                                .With<SM_Welcome>(m =>
                                {
                                    // TODO: process
                                });
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
}
