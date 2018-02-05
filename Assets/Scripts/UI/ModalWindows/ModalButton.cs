using Assets.Scripts.UI.ModalWindows.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ModalWindows
{
    [RequireComponent(typeof(Button))]
    public class ModalButton : MonoBehaviour
    {
        [SerializeField]
        protected Text buttonTitle;
        protected Button button;
        
        public event Action ButtonClicked;

        protected void Awake()
        {
            button = GetComponent<Button>();
        }

        public virtual void Init(ModalButtonParameters parameters)
        {
            buttonTitle.text = parameters.title;
            ButtonClicked += parameters.clickCallback;
        }

        public void ButtonClick()
        {
            if(ButtonClicked != null)
            {
                ButtonClicked();
            }
        }
    }
}
