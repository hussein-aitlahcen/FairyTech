using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyTech.Server.Game
{
    public interface IUpdatable : IDisposable
    {
        void Update(long dt);
        void AddChild(IUpdatable child);
    }
}
