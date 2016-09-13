using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;
using FairyTech.Common.Network.Protocol.Type;

namespace FairyTech.Server.Game.Handler
{
    public delegate void GameMessageHandler<in T>(Player player, T message) where T : AbstractNetworkMessage;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GameHandlerAttribute : Attribute
    {
    }
}
