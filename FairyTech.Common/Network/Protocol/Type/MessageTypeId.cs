using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FairyTech.Common.Network.Protocol.Type
{
    public enum MessageTypeId : int
    {
        SM_WELCOME = 2000,
        SM_AUTHENTICATION_RESULT = 2001,

        CM_AUTHENTICATION_REQUEST = 1000,
    }
}
