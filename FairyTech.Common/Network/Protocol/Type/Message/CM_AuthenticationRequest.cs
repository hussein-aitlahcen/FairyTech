using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace FairyTech.Common.Network.Protocol.Type.Message
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class CM_AuthenticationRequest : AbstractNetworkMessage
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
