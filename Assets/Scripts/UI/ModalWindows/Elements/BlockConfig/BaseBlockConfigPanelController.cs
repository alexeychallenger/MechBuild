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
        public Toggle gravityToggle;
        public Toggle freezeToggle;

        protected Block block;

        public override void Init(BlockConfigPanelParameters parameters)
        {
            base.Init(parameters);
            var panelParameters = parameters as BaseBlockConfigPanelParameters;
            block = panelParameters.block;
            block.MassValueChanged += UpdateTextFields;
            block.UseGravityValueChanged += UpdateTextFields;
            block.IsFreezeValueChanged += UpdateTextFields;
            UpdateTextFields();
        }

        private void OnDestroy()
        {
            if (block != null)
            {
                block.MassValueChanged -= UpdateTextFields;
                block.UseGravityValueChanged -= UpdateTextFields;
                block.IsFreezeValueChanged -= UpdateTextFields;
            }
        }

        private void UpdateTextFields<T>(ChangeValueEventArgs<T> e)
        {
            UpdateTextFields();
        }

        private void UpdateTextFields()
        {
            massInputField.text = massInputField.text = string.Format("{0}", block.Mass);
            gravityToggle.isOn = block.UseGravity;
            freezeToggle.isOn = block.IsFreeze;
        }

        public void SaveMassValue()
        {
            if (!string.IsNullOrEmpty(massInputField.text))
            {
                block.Mass = Convert.ToSingle(massInputField.text);
            }
        }

        public void SaveGravityValue()
        {
            block.UseGravity = gravityToggle.isOn;
        }

        public void SaveFreezeValue()
        {
            block.IsFreeze = freezeToggle.isOn;
        }
    }
}
