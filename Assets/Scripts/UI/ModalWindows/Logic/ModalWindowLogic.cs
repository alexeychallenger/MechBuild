using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI.ModalWindows.Parameters;

namespace Assets.Scripts.UI.ModalWindows.Logic
{
    public abstract class ModalWindowLogic : MonoBehaviour
    {
        [SerializeField]
        protected ModalWindow modalWindow;
        protected Action closeCallback;
        
        public ModalType Type { get; protected set; }
        public bool IsReady { get; protected set; }
        
        public virtual void Init(ModalParameters parameters)
        {
            Type = parameters.Type;
        }

        public virtual Action GetCloseCallback()
        {
            return closeCallback;
        }
        
    }
}


