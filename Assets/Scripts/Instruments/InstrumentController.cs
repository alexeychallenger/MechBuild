using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public class InstrumentController : MonoBehaviour
    {
        protected InstrumentType currentInstrument;

        public Instrument[] instrumentArray;

        public event Action<ChangeValueEventArgs<InstrumentType>> CurrentInstrumentChanged;

        protected void Start()
        {
            CurrentInstrument = currentInstrument;
            SelectInstrument(currentInstrument);
        }

        public InstrumentType CurrentInstrument
        {
            get { return currentInstrument; }
            set
            {
                var oldValue = currentInstrument;
                if (value != oldValue)
                {
                    currentInstrument = value;
                    if (CurrentInstrumentChanged != null)
                    {
                        CurrentInstrumentChanged(new ChangeValueEventArgs<InstrumentType>(oldValue, value));
                    }
                }
            }
        }
        
        public void SelectInstrument(InstrumentType instrumentType)
        {
            CurrentInstrument = instrumentType;
            foreach (Instrument instrument in instrumentArray)
            {
                instrument.SwitchActive(instrument.Type == currentInstrument);
            }
        }
    }
}
