using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class BaseBlockConfigPanelParameters : BlockConfigPanelParameters
    {
        public override BlockConfigPanelType Type
        {
            get
            {
                return BlockConfigPanelType.BaseBlockConfigPanel;
            }
        }

        public Block block;

        public BaseBlockConfigPanelParameters(Block block)
        {
            this.block = block;
        }
    }
}
