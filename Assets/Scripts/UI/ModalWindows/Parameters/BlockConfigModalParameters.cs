using Assets.Scripts.UI.ModalWindows.Elements.BlockConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Parameters
{
    public class BlockConfigModalParameters : ModalParameters
    {
        public override ModalType Type
        {
            get
            {
                return ModalType.BlockConfigModal;
            }
        }

        public List<BlockConfigPanelParameters> panelParametersList;
        public Action closeCallback;

        public BlockConfigModalParameters(Action closeCallback = null)
        {
            this.closeCallback = closeCallback;
            panelParametersList = new List<BlockConfigPanelParameters>();
        }
    }
}
