using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using UnityEngine.UI;
using Assets.Scripts.UI.ModalWindows.Parameters;
using Assets.Scripts.GameManagement;

namespace Assets.Scripts.UI.ModalWindows.Logic
{
    public class InfoModal : ModalWindowLogic
    {
        [SerializeField]
        protected Text titleText;
        [SerializeField]
        protected Text messageText;
        [SerializeField]
        protected GameObject buttonPrefab;
        [SerializeField]
        protected Transform buttonsPanel;

        public override void Init(ModalParameters parameters) 
        {
            base.Init(parameters);
            InfoModalParameters infoModalParameters = parameters as InfoModalParameters;

            if (!string.IsNullOrEmpty(infoModalParameters.Title))
            {
                titleText.text = infoModalParameters.Title;
            }
            if (!string.IsNullOrEmpty(infoModalParameters.Message))
            {
                messageText.text = infoModalParameters.Message;
            }
            if (infoModalParameters.Buttons != null)
            {
                foreach(ModalButtonParameters modalButtonParameters in infoModalParameters.Buttons)
                {
                    if (modalButtonParameters == null)
                    {
                        SpawnButton(GenerateDefaultButtonParameters());
                    }
                    else
                    {
                        ModalButton modalButton = SpawnButton(modalButtonParameters);
                        modalButton.ButtonClicked += modalWindow.CloseWindow;
                    }
                }
            }
            else
            {
                SpawnButton(GenerateDefaultButtonParameters());
            }
            closeCallback = infoModalParameters.CloseCallback;
            IsReady = true;
        }

        protected virtual ModalButtonParameters GenerateDefaultButtonParameters()
        {
            return new ModalButtonParameters
                    (
                        title: "Close",
                        clickCallback: modalWindow.CloseWindow
                    );
        }

        protected virtual ModalButton SpawnButton(ModalButtonParameters parameters)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, buttonsPanel);
            ModalButton modalButton = buttonObject.GetComponent<ModalButton>();
            modalButton.Init(parameters);
            return modalButton;
        }
    }
}


