using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class LimitBlockConfigPanelParameters : BlockConfigPanelParameters
    {
        public override BlockConfigPanelType Type
        {
            get
            {
                return BlockConfigPanelType.LimitBlockConfigPanel;
            }
        }

        public LimitControl limitControl;

        public LimitBlockConfigPanelParameters(LimitControl limitControl)
        {
            this.limitControl = limitControl;
        }
    }
}
