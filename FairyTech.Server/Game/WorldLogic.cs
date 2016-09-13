using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;
using FairyTech.Common.Network.Protocol.Type.Message;
using FairyTech.Server.Game.Handler;
using FairyTech.Server.Network;
using Lidgren.Network;

namespace FairyTech.Server.Game
{
    public sealed class WorldLogic : Updatable
    {
        public static TimeSpan UpdateDelay = TimeSpan.FromMilliseconds(10);

        private readonly GameServer m_server;
        private readonly Stopwatch m_worldWatch;

        public WorldLogic()
        {
            m_server = new GameServer();
            m_worldWatch = new Stopwatch();
        }

        public void Start()
        {
            m_server.Start();
            m_worldWatch.Start();

            Tick();
        }

        public void Tick()
        {
            var beginTime = m_worldWatch.ElapsedMilliseconds;

            var dt = beginTime - GameTime;

            try
            {
                HandleClientMessages();
            }
            catch (Exception e)
            {
            }

            Update(dt);

            var endTime = m_worldWatch.ElapsedMilliseconds;
            var lag = TimeSpan.FromMilliseconds(endTime - beginTime);
            var delay = lag >= UpdateDelay ? TimeSpan.Zero : UpdateDelay - lag;

            if (delay > TimeSpan.Zero)
                Task.Delay(delay);

            Task.Factory.StartNew(Tick);
        }

        private void HandleClientMessages()
        {
            var messages = new List<NetIncomingMessage>();
            m_server.ReadMessages(messages);
            foreach (var message in messages)
            {
                Console.WriteLine("message received < " + message.MessageType);
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        var text = message.ReadString();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                var outgoing = message.SenderConnection.Peer.CreateMessage();
                                outgoing.Write(new SM_Welcome().Serialize());
                                message.SenderConnection.SendMessage(outgoing, NetDeliveryMethod.ReliableOrdered, 0);
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        var deserialized = AbstractNetworkMessage.Deserialize(message.ReadBytes(message.LengthBytes));
                        HandlerInitializer.Handle(message.SenderConnection.Tag as Player, deserialized);
                        break;
                }
            }
        }
    }
}
