using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace FairyTech.Common.Network.Protocol.Type
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public enum AuthenticationFailureReason
    {
        WRONG_CREDENTIALS = 0,
        TEMPORARY_BANNED = 1,
        PERMANENTLY_BANNED = 2,
        PROTOCOL_REQUIRED = 3,
    }
}
