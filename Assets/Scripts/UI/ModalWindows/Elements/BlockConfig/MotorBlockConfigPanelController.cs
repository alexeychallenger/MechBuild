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
    public class MotorBlockConfigPanelController : BlockConfigPanelController
    {
        public InputField velocityInputField;
        public InputField forceInputField;
        public InputField forwardKeyInputField;
        public InputField backKeyInputField;
        public Toggle freeSpinToggle;
        public Toggle fixableToggle;
        public Toggle reverseToggle;

        protected KeyCode forwardKeyBackup;
        protected KeyCode backKeyBackup;

        protected MotorControl motorControl;

        public override void Init(BlockConfigPanelParameters parameters)
        {
            base.Init(parameters);
            var panelParameters = parameters as MotorBlockConfigPanelParameters;
            motorControl = panelParameters.motorControl;

            motorControl.MotorVelocityChanged += UpdateTextFields;
            motorControl.MotorForceChanged += UpdateTextFields;
            motorControl.FreespinChanged += UpdateTextFields;
            motorControl.Reverced += UpdateTextFields;
            motorControl.ForwardAxisKeyChanged += UpdateTextFields;
            motorControl.BackAxisKeyChanged += UpdateTextFields;
            motorControl.IsFixableValueChanged += UpdateTextFields;

            UpdateTextFields();
        }

        protected void UpdateTextFields<T>(ChangeValueEventArgs<T> e)
        {
            UpdateTextFields();
        }

        protected void UpdateTextFields()
        {
            velocityInputField.text = string.Format("{0}", motorControl.MotorVelocity);
            forceInputField.text = string.Format("{0}", motorControl.MotorForce);
            freeSpinToggle.isOn = motorControl.Freespin;
            reverseToggle.isOn = motorControl.IsReverse;
            fixableToggle.isOn = motorControl.IsFixable;
            forwardKeyBackup = motorControl.ForwardAxisKey;
            forwardKeyInputField.text = string.Format("{0}", motorControl.ForwardAxisKey);
            backKeyBackup = motorControl.BackAxisKey;
            backKeyInputField.text = string.Format("{0}", motorControl.BackAxisKey);
        }

        protected void OnDestroy()
        {
            motorControl.MotorVelocityChanged -= UpdateTextFields;
            motorControl.MotorForceChanged -= UpdateTextFields;
            motorControl.FreespinChanged -= UpdateTextFields;
            motorControl.Reverced -= UpdateTextFields;
            motorControl.ForwardAxisKeyChanged -= UpdateTextFields;
            motorControl.BackAxisKeyChanged -= UpdateTextFields;
            motorControl.IsFixableValueChanged -= UpdateTextFields;
        }

        public void SaveVelocity()
        {
            motorControl.MotorVelocity = Convert.ToSingle(velocityInputField.text);
        }

        public void SaveForce()
        {
            motorControl.MotorForce = Convert.ToSingle(forceInputField.text);
        }

        public void SaveFreeSpin()
        {
            motorControl.Freespin = freeSpinToggle.isOn;
        }

        public void SaveFixable()
        {
            motorControl.IsFixable = fixableToggle.isOn;
        }

        public void SaveReverce()
        {
            motorControl.IsReverse = reverseToggle.isOn;
        }

        public void SaveForwardKey()
        {
            try
            {
                KeyCode newKey = (KeyCode)Enum.Parse(typeof(KeyCode), forwardKeyInputField.text);
                motorControl.ForwardAxisKey = newKey;
            }
            catch
            {
                forwardKeyInputField.text = string.Format("{0}", motorControl.ForwardAxisKey);
            }
        }

        public void SaveBackKey()
        {
            try
            {
                KeyCode newKey = (KeyCode)Enum.Parse(typeof(KeyCode), backKeyInputField.text);
                motorControl.BackAxisKey = newKey;
            }
            catch
            {
                backKeyInputField.text = string.Format("{0}", motorControl.BackAxisKey);
            }
        }
    }
}
