using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyTech.Server.Game
{
    public abstract class Updatable : IUpdatable
    {
        public long GameTime
        {
            get;
            private set;
        }

        private readonly List<IUpdatable> m_childs = new List<IUpdatable>();

        public void AddChild(IUpdatable child)
        {
            m_childs.Add(child);
        }

        public void Update(long dt)
        {
            GameTime += dt;
            
            foreach (var child in m_childs.ToArray())
                child.Update(dt);
        }

        public void Dispose()
        {
            foreach(var child in m_childs)
                child.Dispose();
            m_childs.Clear();
        }
    }
}
