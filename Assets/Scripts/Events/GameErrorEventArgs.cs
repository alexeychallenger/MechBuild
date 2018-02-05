using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class GameErrorEventArgs : EventArgs
    {
        public string Message { get; protected set; }
        
        public GameErrorEventArgs(string message)
        {
            Message = message;
        }
    }
}
