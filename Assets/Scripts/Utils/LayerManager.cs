using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class LayerManager
    {
        public static LayerMask ignorePreviewLayerMask;
        public static LayerMask blockLayerMask;

        static LayerManager()
        {
            ConfigLayerMasks();
        }


        public static void SwitchLayer(GameObject gameObject, LayerType layerType)
        {
            gameObject.layer = ToLayerMask(layerType);
        }

        public static LayerMask ToLayerMask(LayerType layerType)
        {
            return LayerMask.NameToLayer(layerType.ToString());
        }



        public static void ConfigLayerMasks()
        {
            ignorePreviewLayerMask = ~(1 << ToLayerMask(LayerType.Preview));
            blockLayerMask = (1 << ToLayerMask(LayerType.Block));
        }
    }
}
