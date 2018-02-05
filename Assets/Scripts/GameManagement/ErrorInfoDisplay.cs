using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Events;
using Assets.Scripts.UI.ModalWindows;
using Assets.Scripts.Utils;
using Assets.Scripts.FileManagement;
using Assets.Scripts.UI.ModalWindows.Parameters;

namespace Assets.Scripts.GameManagement
{
    /// <summary>
    /// Observes error events and displays error info if error has occurred
    /// </summary>
    public class ErrorInfoDisplay : MonoBehaviour
    {
        public ModalManager modalManager;

        protected void Awake()
        {
            FileWriter.ErrorOccurred += DisplayError;
            FileDownloader.ErrorOccurred += DisplayError;
            SerializeUtils<ISerializationCallbackReceiver>.ErrorOccurred += DisplayError;
        }

        /// <summary>
        /// Display info modal with error info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DisplayError(GameErrorEventArgs e)
        {
            DbLog.LogError(e.Message);
            modalManager.CreateModal(new InfoModalParameters("Error!", e.Message));
        }
    }
}
