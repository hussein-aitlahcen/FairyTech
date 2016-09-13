using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;

namespace FairyTech.Server.Game.Handler
{
    public sealed class GameHandler
    {
        private readonly Dictionary<Type, Action<Player, AbstractNetworkMessage>> m_handlers =
            new Dictionary<Type, Action<Player, AbstractNetworkMessage>>();
         
        public void Initialize()
        {
            foreach (
                var type in
                    typeof (GameHandler).Module.GetTypes()
                        .Where(t => t.GetCustomAttribute(typeof (GameHandlerAttribute)) != null))
            {
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(
                    f => f.FieldType.IsAssignableFrom(typeof (GameMessageHandler<AbstractNetworkMessage>))))
                {
                    var handler = field.GetValue(null);
                    var genericType = field.FieldType.GenericTypeArguments[0];
                    m_handlers[genericType] = (player, message) =>
                    {
                        var changed = Convert.ChangeType(message, genericType);
                        ((dynamic)handler).DynamicInvoke(player, changed);
                    };
                }
            }
        }

        public void Handle(Player player, AbstractNetworkMessage message)
        {
            m_handlers[message.GetType()](player, message);
        }
    }
}
