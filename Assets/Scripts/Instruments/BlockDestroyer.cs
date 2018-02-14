using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Instruments
{
    public class BlockDestroyer : Instrument
    {
        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Destroyer;
            }
        }
    }
}
