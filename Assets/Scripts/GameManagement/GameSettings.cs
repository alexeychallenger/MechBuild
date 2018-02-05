using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
    [Serializable]
    public struct GameSettings : ISerializationCallbackReceiver
    {
        public float volume;
        public bool mute;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
