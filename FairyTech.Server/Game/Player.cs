using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;
using Lidgren.Network;

namespace FairyTech.Server.Game
{
    public sealed class Player
    {
        public NetConnection Connection
        {
            get;
            private set;
        }

        public Player(NetConnection connection)
        {
            Connection = connection;
            Connection.Tag = this;
        }

        public void SendReliable(AbstractNetworkMessage message)
        {
            var serialized = message.Serialize();
            var outgoing = Connection.Peer.CreateMessage(serialized.Length);
            outgoing.Write(serialized.Length);
            outgoing.Write(serialized);
            Connection.SendMessage(outgoing, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
