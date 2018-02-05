using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI.ModalWindows.Logic;
using Assets.Scripts.Events;

namespace Assets.Scripts.UI.ModalWindows
{
    public class ModalWindow : MonoBehaviour
    {
        [SerializeField]
        private GameObject modalWindowContainer;
        [SerializeField]
        public ModalWindowLogic modalWindowLogic;

        public static Action<ModalWindowEventArgs> modalWindowOpened;
        public static Action<ModalWindowEventArgs> modalWindowClosed;

        public bool IsOpen { get; protected set; }

        protected Action closeCallback;

        private void Awake()
        {
            modalWindowContainer.SetActive(false);
        }

        private void Start()
        {
            OpenWindow();
        }

        public void OpenWindow()
        {
            StartCoroutine(OpenWindowAsync());
        }

        /// <summary>
        /// Open window
        /// </summary>
        public IEnumerator OpenWindowAsync()
        {
            modalWindowLogic.enabled = true;
            //TODO open window animation

            yield return new WaitUntil(() => (modalWindowLogic.IsReady || modalWindowLogic == null));
            closeCallback = modalWindowLogic.GetCloseCallback();
            modalWindowContainer.SetActive(true);
            IsOpen = true;
            if (modalWindowOpened != null)
            {
                modalWindowOpened(new ModalWindowEventArgs(this, modalWindowLogic));
            }
        }

        public void CloseWindow()
        {
            StartCoroutine(CloseWindowAsync());
        }

        /// <summary>
        /// Close window and destroy window gameobject
        /// </summary>
        public IEnumerator CloseWindowAsync()
        {
            //TODO close window animation
            yield return null;
            //Calling close callback
            if (closeCallback != null)
            {
                closeCallback();
            }
            IsOpen = false;
            if (modalWindowClosed != null)
            {
                modalWindowClosed(new ModalWindowEventArgs(this, modalWindowLogic));
            }
            Destroy(gameObject);
        }


    }
}


