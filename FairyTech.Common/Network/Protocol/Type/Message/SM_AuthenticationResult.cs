using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace FairyTech.Common.Network.Protocol.Type.Message
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SM_AuthenticationResult : AbstractNetworkMessage
    {
        public bool Success { get; set; }
        public AuthenticationFailureReason Reason { get; set; }
        public string Parameter { get; set; }
    }
}
