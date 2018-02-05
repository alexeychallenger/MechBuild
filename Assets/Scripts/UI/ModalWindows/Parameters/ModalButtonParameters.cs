using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using Assets.Scripts.UI.ModalWindows;

namespace Assets.Scripts.UI.ModalWindows.Parameters
{
    public class ModalButtonParameters
    {
        public string title;
        public Action clickCallback;

        public ModalButtonParameters(string title, Action clickCallback)
        {
            this.title = title;
            this.clickCallback = clickCallback;
        }
    }
}
