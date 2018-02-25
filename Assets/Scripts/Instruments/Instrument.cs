using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public abstract class Instrument : MonoBehaviour
    {
        public abstract InstrumentType Type { get; }

        public event Action<bool> ActiveStateSwitched;

        public void SwitchActive(bool state)
        {
            if (state != gameObject.activeSelf)
            {
                gameObject.SetActive(state);
                if (ActiveStateSwitched != null)
                {
                    ActiveStateSwitched(gameObject.activeSelf);
                }
            }
        }
    }
}
