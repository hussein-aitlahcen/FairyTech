using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace FairyTech.Common.Network.Protocol.Type.Message
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SM_Welcome : AbstractNetworkMessage
    {
        public short ProtocolVersion { get; set; }
    }
}
