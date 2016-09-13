using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol;

namespace FairyTech.Server.Game.Handler
{
    public static class HandlerInitializer
    {
        private static Dictionary<Type, Action<Player, AbstractNetworkMessage>> Handlers =
            new Dictionary<Type, Action<Player, AbstractNetworkMessage>>();
         
        public static void Initialize()
        {
            foreach (
                var type in
                    typeof (HandlerInitializer).Module.GetTypes()
                        .Where(t => t.GetCustomAttribute(typeof (GameHandlerAttribute)) != null))
            {
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(
                    f => f.FieldType.IsAssignableFrom(typeof (GameMessageHandler<AbstractNetworkMessage>))))
                {
                    var handler = field.GetValue(null);
                    var genericType = field.FieldType.GenericTypeArguments[0];
                    Handlers[genericType] = (player, message) =>
                    {
                        var changed = Convert.ChangeType(message, genericType);
                        ((dynamic)handler).DynamicInvoke(player, changed);
                    };
                }
            }
        }

        public static void Handle(Player player, AbstractNetworkMessage message)
        {
            Handlers[message.GetType()](player, message);
        }
    }
}
