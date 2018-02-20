using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class HingeBlockConfigPanelController : BlockConfigPanelController
    {
        public override void Init(BlockConfigPanelParameters parameters)
        {
            base.Init(parameters);
            var panelParameters = parameters as HingeBlockConfigPanelParameters;
        }
    }
}
