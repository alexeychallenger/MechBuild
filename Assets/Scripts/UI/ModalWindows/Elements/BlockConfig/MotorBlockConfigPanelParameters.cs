using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class MotorBlockConfigPanelParameters : BlockConfigPanelParameters
    {
        public override BlockConfigPanelType Type
        {
            get
            {
                return BlockConfigPanelType.MotorBlockConfigPanel;
            }
        }

        public MotorControl motorControl;

        public MotorBlockConfigPanelParameters(MotorControl motorControl)
        {
            this.motorControl = motorControl;
        }
    }
}
