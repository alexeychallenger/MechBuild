using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Instrument
{
    public class PanelSwitcher : MonoBehaviour
    {
        public Instruments.Instrument instrument;

        private void Awake()
        {
            instrument.ActiveStateSwitched += SwithcActiveState;
        }

        private void OnDestroy()
        {
            instrument.ActiveStateSwitched -= SwithcActiveState;
        }

        private void SwithcActiveState(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
