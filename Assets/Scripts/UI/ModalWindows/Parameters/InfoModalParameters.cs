using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Parameters
{
    public class InfoModalParameters: ModalParameters
    {
        public override ModalType Type
        {
            get
            {
                return ModalType.InfoModal;
            }
        }

        public string Title { get; protected set; }
        public string Message { get; protected set; }
        public Action CloseCallback { get; protected set; }
        public ModalButtonParameters[] Buttons { get; protected set; }

        public InfoModalParameters(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public InfoModalParameters(string title, string message, ModalButtonParameters[] buttons) : this(title, message)
        {
            Buttons = buttons;
        }

        public InfoModalParameters(string title, string message, Action closeCallback, ModalButtonParameters[] buttons) : this(title, message, buttons)
        {
            CloseCallback = closeCallback;
        }

        public InfoModalParameters(string title, string message, Action closeCallback) : this(title, message)
        {
            CloseCallback = closeCallback;
        }
    }
}
