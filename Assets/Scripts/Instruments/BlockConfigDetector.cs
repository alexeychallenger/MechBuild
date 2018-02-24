using Assets.Scripts.Blocks;
using Assets.Scripts.UI.ModalWindows.Elements.BlockConfig;
using Assets.Scripts.UI.ModalWindows.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public static class BlockConfigDetector
    {
        public static BlockConfigModalParameters GetBlockConfigModalParameters(Block block)
        {
            BlockConfigModalParameters parameters = new BlockConfigModalParameters();
            parameters.panelParametersList.Add(new BaseBlockConfigPanelParameters(block));

            GameObject blockObject = block.gameObject;
            CheckHingeBlock(parameters, blockObject);
            CheckMotorControl(parameters, blockObject);
            CheckLimitControl(parameters, blockObject);
            return parameters;
        }

        private static void CheckLimitControl(BlockConfigModalParameters parameters, GameObject blockObject)
        {
            LimitControl limitControl = blockObject.GetComponent<LimitControl>();
            if (limitControl != null)
            {
                parameters.panelParametersList.Add(new LimitBlockConfigPanelParameters(limitControl));
            }
        }

        private static void CheckMotorControl(BlockConfigModalParameters parameters, GameObject blockObject)
        {
            MotorControl motorControl = blockObject.GetComponent<MotorControl>();
            if (motorControl != null)
            {
                parameters.panelParametersList.Add(new MotorBlockConfigPanelParameters(motorControl));
            }
        }

        private static void CheckHingeBlock(BlockConfigModalParameters parameters, GameObject blockObject)
        {
            HingeBlock hingeBlockComponent = blockObject.GetComponent<HingeBlock>();
            if (hingeBlockComponent != null)
            {
                parameters.panelParametersList.Add(new HingeBlockConfigPanelParameters(hingeBlockComponent));
            }
        }
    }
}
