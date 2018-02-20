using Assets.Scripts.Blocks;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ModalWindows.Elements.BlockConfig
{
    public class BaseBlockConfigPanelController : BlockConfigPanelController
    {
        public InputField massInputField;

        protected Block block;

        public override void Init(BlockConfigPanelParameters parameters)
        {

            base.Init(parameters);
            var panelParameters = parameters as BaseBlockConfigPanelParameters;
            block = panelParameters.block;
            block.MassChanged += UpdateMassText;
            
            UpdateMassText(block.Mass);
        }

        private void OnDestroy()
        {
            if (block != null)
            {
                block.MassChanged -= UpdateMassText;
            }
        }

        private void UpdateMassText(ChangeValueEventArgs<float> e)
        {
            UpdateMassText(e.NewValue);
        }

        private void UpdateMassText(float newValue)
        {
            massInputField.text = massInputField.text = string.Format("{0}", newValue);
        }

        public void SaveMassValue()
        {
            if (!string.IsNullOrEmpty(massInputField.text))
            {
                block.Mass = Convert.ToSingle(massInputField.text);
            }
        }
    }
}
