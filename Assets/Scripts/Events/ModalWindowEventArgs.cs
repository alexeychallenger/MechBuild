using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.UI.ModalWindows;
using Assets.Scripts.UI.ModalWindows.Logic;

namespace Assets.Scripts.Events
{
    public class ModalWindowEventArgs : EventArgs
    {
        public ModalWindow modalWindow;
        public ModalWindowLogic modalWindowLogic;

        public ModalWindowEventArgs(ModalWindow modalWindow, ModalWindowLogic modalWindowLogic)
        {
            this.modalWindow = modalWindow;
            this.modalWindowLogic = modalWindowLogic;
        }
    }
}
