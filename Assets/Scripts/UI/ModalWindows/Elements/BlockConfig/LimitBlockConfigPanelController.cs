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
    public class LimitBlockConfigPanelController : BlockConfigPanelController
    {
        public InputField minLimitInputText;
        public InputField maxLimitInputText;
        public Toggle useLimitsToggle;

        protected LimitControl limitControl;

        public override void Init(BlockConfigPanelParameters parameters)
        {

            base.Init(parameters);
            var panelParameters = parameters as LimitBlockConfigPanelParameters;
            limitControl = panelParameters.limitControl;
            limitControl.MinLimitChanged += UpdateTextFields;
            limitControl.MaxLimitChanged += UpdateTextFields;
            limitControl.UseLimitsChanged += UpdateTextFields;
            UpdateTextFields();
        }

        private void OnDestroy()
        {
            limitControl.MinLimitChanged -= UpdateTextFields;
            limitControl.MaxLimitChanged -= UpdateTextFields;
            limitControl.UseLimitsChanged -= UpdateTextFields;
        }

        private void UpdateTextFields<T>(ChangeValueEventArgs<T> e)
        {
            UpdateTextFields();
        }

        private void UpdateTextFields()
        {
            minLimitInputText.text = string.Format("{0}", limitControl.MinLimit);
            maxLimitInputText.text = string.Format("{0}", limitControl.MaxLimit);
            useLimitsToggle.isOn = limitControl.UseLimits;
        }

        public void SaveMinLimitValue()
        {
            limitControl.MinLimit = Convert.ToSingle(minLimitInputText.text);
        }

        public void SaveMaxLimitValue()
        {
            limitControl.MaxLimit = Convert.ToSingle(maxLimitInputText.text);
        }

        public void SaveUseLimitsLimitValue()
        {
            limitControl.UseLimits = useLimitsToggle.isOn;
        }
    }
}
