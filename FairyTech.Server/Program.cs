using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyTech.Common.Network.Protocol.Type.Message;
using FairyTech.Server.Game;
using FairyTech.Server.Game.Handler;

namespace FairyTech.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            new WorldLogic().Start();

            Console.ReadLine();
        }
    }
}
