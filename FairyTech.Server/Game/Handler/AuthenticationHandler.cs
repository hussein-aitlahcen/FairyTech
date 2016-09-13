using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;
using FairyTech.Common.Network.Protocol.Type;
using FairyTech.Common.Network.Protocol.Type.Message;

namespace FairyTech.Server.Game.Handler
{
    [GameHandler]
    public static class AuthenticationHandler
    {
        private static GameMessageHandler<CM_AuthenticationRequest>
            AuthRequestHandler = (player, request) =>
            {
                
            };
    }
}
