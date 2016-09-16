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

        private readonly GameHandler m_handler;
        private readonly GameServer m_server;
        private readonly Stopwatch m_worldWatch;
        private readonly HashSet<Player> m_players;  

        public WorldLogic()
        {
            m_handler = new GameHandler();
            m_server = new GameServer();
            m_worldWatch = new Stopwatch();
            m_players = new HashSet<Player>();
        }

        public void Start()
        {
            m_handler.Initialize();
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
                Console.WriteLine(e.ToString());
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
                                OnPlayerConnected(new Player(message.SenderConnection));
                                break;
                            case NetConnectionStatus.None:
                                break;
                            case NetConnectionStatus.InitiatedConnect:
                                break;
                            case NetConnectionStatus.ReceivedInitiation:
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                break;
                            case NetConnectionStatus.Disconnecting:
                                break;
                            case NetConnectionStatus.Disconnected:
                                OnPlayerDisconnected((Player)message.SenderConnection.Tag);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        OnPlayerMessage((Player) message.SenderConnection.Tag, AbstractNetworkMessage.Deserialize(message.ReadBytes(message.ReadInt32())));
                        break;


                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                }
            }
        }

        private void OnPlayerMessage(Player player, AbstractNetworkMessage message)
        {
            Console.WriteLine("received << " + message.GetType().Name);
            m_handler.Handle(player, message);
        }

        private void OnPlayerConnected(Player player)
        {
            m_players.Add(player);
        }

        private void OnPlayerDisconnected(Player player)
        {
            m_players.Remove(player);
        }
    }
}
