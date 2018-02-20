using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class HingeBlockConfigPanelParameters : BlockConfigPanelParameters
    {
        public override BlockConfigPanelType Type
        {
            get
            {
                return BlockConfigPanelType.HingeBlockConfigPanel;
            }
        }

        public HingeBlock hingeBlockComponent;

        public HingeBlockConfigPanelParameters(HingeBlock hingeBlockComponent)
        {
            this.hingeBlockComponent = hingeBlockComponent;
        }
    }
}
