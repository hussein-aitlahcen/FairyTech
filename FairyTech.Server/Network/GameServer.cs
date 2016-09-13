using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Server.Game.Handler;
using Lidgren.Network;

namespace FairyTech.Server.Network
{
    public sealed class GameServer : NetServer
    {
        public GameServer() : base(new NetPeerConfiguration("FairyTech")
        {
            Port = 2648
        })
        {
        }
    }
}
