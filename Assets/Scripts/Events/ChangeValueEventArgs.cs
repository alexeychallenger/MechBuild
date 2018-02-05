using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class ChangeValueEventArgs<T> : EventArgs
    {
        public T OldValue { get; protected set; }
        public T NewValue { get; protected set; }

        public ChangeValueEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
