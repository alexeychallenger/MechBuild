using Assets.Scripts.GameManagement;
using Assets.Scripts.UI.ModalWindows.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ModalWindows.Logic
{
    public class SettingsModal : ModalWindowLogic
    {
        protected GameData gameData;

        [SerializeField]
        protected Toggle soundToggle;
        [SerializeField]
        protected Toggle musicToggle;

        public override void Init(ModalParameters parameters)
        {
            SettingsModalParameters settingsModalParameters = parameters as SettingsModalParameters;
            gameData = settingsModalParameters.gameData;
            closeCallback = settingsModalParameters.closeCallback;
            IsReady = true;
        }
    }
}
