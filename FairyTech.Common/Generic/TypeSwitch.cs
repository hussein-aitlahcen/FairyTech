using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FairyTech.Common.Generic
{
    public sealed class TypeSwitch
    {
        private readonly object m_object;
        private bool m_handled;

        public TypeSwitch(object obj)
        {
            m_object = obj;
            m_handled = false;
        }

        public TypeSwitch With<T>(Action<T> fun)
            where T : class
        {
            if (m_handled)
                return this;

            var typed = m_object as T;
            if (typed != null)
            {
                m_handled = true;
                fun(typed);
            }

            return this;
        }
    }
}
