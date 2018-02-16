using Assets.Scripts.Blocks;
using Assets.Scripts.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Instrument
{
    public class BlockSelectButton : MonoBehaviour
    {
        protected Block blockPrefab;
        protected BlockSpawner blockSpawner;

        public Image blockImage;
        public Text blockName;

        public void Init(Block blockPrefab, BlockSpawner blockSpawner)
        {
            this.blockPrefab = blockPrefab;
            this.blockSpawner = blockSpawner;
            blockName.text = blockPrefab.name;
        }

        public void OnButtonClick()
        {
            blockSpawner.blockPrefab = blockPrefab;
        }
    }
}
