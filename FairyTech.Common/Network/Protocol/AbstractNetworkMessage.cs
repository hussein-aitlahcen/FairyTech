using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FairyTech.Common.Network.Protocol.Type;
using FairyTech.Common.Network.Protocol.Type.Message;
using ProtoBuf;

namespace FairyTech.Common.Network.Protocol
{
    [ProtoInclude((int)MessageTypeId.SM_AUTHENTICATION_RESULT, typeof(SM_AuthenticationResult))]
    [ProtoInclude((int)MessageTypeId.CM_AUTHENTICATION_REQUEST, typeof(CM_AuthenticationRequest))]
    [ProtoInclude((int)MessageTypeId.SM_WELCOME, typeof(SM_Welcome))]
    [ProtoContract]
    public abstract class AbstractNetworkMessage
    {  
        private byte[] m_serializedBuffer;

        public byte[] Serialize()
        {
            if (m_serializedBuffer == null)
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    m_serializedBuffer = stream.ToArray();
                }
            }
            return m_serializedBuffer;
        }
        
        public static AbstractNetworkMessage Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
                return Serializer.Deserialize<AbstractNetworkMessage>(stream);
        }
    }
}
