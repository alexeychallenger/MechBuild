using Assets.Scripts.GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Parameters
{
    public class SettingsModalParameters : ModalParameters
    {
        public override ModalType Type
        {
            get
            {
                return ModalType.SettingsModal;
            }
        }

        public GameData gameData;
        public Action closeCallback;

        public SettingsModalParameters(GameData gameData, Action closeCallback)
        {
            this.gameData = gameData;
            this.closeCallback = closeCallback;
        }
    }
}
