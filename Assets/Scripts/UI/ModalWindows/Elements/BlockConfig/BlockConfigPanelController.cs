using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public abstract class BlockConfigPanelController : MonoBehaviour
    {
        public BlockConfigPanelType Type { get; protected set; }

        public virtual void Init(BlockConfigPanelParameters blockConfigPanelParameters)
        {
            Type = blockConfigPanelParameters.Type;
        }
    }
}
