using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
