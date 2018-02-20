using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.UI.ModalWindows.Elements.BlockConfig;
using Assets.Scripts.UI.ModalWindows.Parameters;
using UnityEngine;

namespace Assets.Scripts.UI.ModalWindows.Logic
{
    public class BlockConfigModal : ModalWindowLogic
    {
        public RectTransform containerRectTransform;

        public List<BlockConfigPanelController> panelList;

        public override void Init(ModalParameters parameters)
        {
            panelList = new List<BlockConfigPanelController>();

            BlockConfigModalParameters blockConfigModalParameters = parameters as BlockConfigModalParameters;
            closeCallback = blockConfigModalParameters.closeCallback;
            foreach (BlockConfigPanelParameters panelParameter in blockConfigModalParameters.panelParametersList)
            {
                SpawnBlockConfigPanel(panelParameter);
            }

            base.Init(parameters);
        }

        protected void SpawnBlockConfigPanel(BlockConfigPanelParameters parameters)
        {
            BlockConfigPanelController panelPrefab = LoadPanelPrefab(parameters);
            if (panelPrefab == null) return;

            BlockConfigPanelController panel = Instantiate(panelPrefab, containerRectTransform);
            panel.Init(parameters);
            panelList.Add(panel);
        }

        protected BlockConfigPanelController LoadPanelPrefab(BlockConfigPanelParameters parameters)
        {
            string path = string.Format("Prefabs/ModalWindows/BlockConfigModalPanels/{0}", parameters.Type);
            BlockConfigPanelController panelController = Resources.Load<BlockConfigPanelController>(path);
            if (panelController == null)
            {
                DbLog.LogWarningFormat("Block config panel prefab not found at path ({0})", path);
            }
            return panelController;
        }
    }
}
