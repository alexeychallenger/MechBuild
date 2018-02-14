using Assets.Scripts.Events;
using Assets.Scripts.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Instrument
{
    [RequireComponent (typeof(Button))]
    public  class InstrumentButton : MonoBehaviour
    {
        public InstrumentController instrumentController;

        public InstrumentType instrumentType;

        protected Button button;

        protected void Awake()
        {
            button = GetComponent<Button>();
            instrumentController.CurrentInstrumentChanged += SwitchButtonInteraction;
        }

        public void SelectInstrument()
        {
            instrumentController.SelectInstrument(instrumentType);
        }

        protected void SwitchButtonInteraction(ChangeValueEventArgs<InstrumentType> e)
        {
            button.interactable = e.NewValue != instrumentType;
        }
    }
}
