using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.UI.ModalWindows.Parameters;
using Assets.Scripts.Events;

namespace Assets.Scripts.UI.ModalWindows
{
    public class ModalManager : MonoBehaviour
    {
        /// <summary>
        /// Modal windows' target parent
        /// </summary>
        private Canvas modalCanvas;

        /// <summary>
        /// resource folder where are located modal window prefabs 
        /// </summary>
        private const string MODAL_PREFAB_RESOURCE_DIR = @"Prefabs/ModalWindows";
        /// <summary>
        /// Signalize that modal window is open  
        /// </summary>
        public bool IsModalWindowOpen { get; protected set; }
        /// <summary>
        /// List of open modal windows
        /// </summary>
        public List<ModalWindow> ModalWindows { get; protected set; }


        protected void Awake()
        {
            modalCanvas = GameObject.FindGameObjectWithTag(Tags.ModalCanvas.ToString()).GetComponent<Canvas>();
            ModalWindows = new List<ModalWindow>();

            ModalWindow.modalWindowOpened += AddModal;
            ModalWindow.modalWindowClosed += RemoveModal;
        }

        protected void OnDestroy()
        {
            ModalWindow.modalWindowOpened -= AddModal;
            ModalWindow.modalWindowClosed -= RemoveModal;
        }
        /// <summary>
        /// Add modal window to list of modal windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddModal(ModalWindowEventArgs e)
        {
            ModalWindows.Add(e.modalWindow);
            CheckActiveWindows();
        }
        /// <summary>
        /// Remove modal window from list of modal windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveModal(ModalWindowEventArgs e)
        {
            ModalWindows.Remove(e.modalWindow);
            CheckActiveWindows();
        }
        /// <summary>
        /// Check for open modal windows
        /// </summary>
        private void CheckActiveWindows()
        {
            IsModalWindowOpen = ModalWindows.Count > 0;
        }

        /// <summary>
        /// Instantiate modal window prefab and return its ModalWindow component
        /// </summary>
        /// <param name="windowType">Modal window type to load from prefab</param>
        /// <returns></returns>
        private ModalWindow InstantiateModalPrefab(ModalType windowType)
        {
            GameObject prefab = Resources.Load<GameObject>(string.Format("{0}/{1}", MODAL_PREFAB_RESOURCE_DIR, windowType.ToString()));
            GameObject windowObject = Instantiate(prefab, modalCanvas.transform);
            windowObject.name = string.Format("{0} window", windowType);

            ModalWindow modalWindow = windowObject.GetComponent<ModalWindow>();
            return modalWindow;
        }

        /// <summary>
        /// Create and initialize modal window which type depends on modal parameters 
        /// </summary>
        /// <param name="parameters">Modal window parameters to initialize. Type of modal window depends on type of modal window parameters</param>
        public void CreateModal(ModalParameters parameters)
        {
            ModalWindow modalWindow = InstantiateModalPrefab(parameters.Type);
            modalWindow.modalWindowLogic.Init(parameters);
        }
    }
}
