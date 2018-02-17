using Assets.Scripts.Blocks;
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
    public class BlockSelectButton : MonoBehaviour
    {
        protected Block blockPrefab;
        protected BlockSpawner blockSpawner;

        public Button button;
        public Image blockImage;
        public Text blockName;

        public void Init(Block blockPrefab, BlockSpawner blockSpawner)
        {
            this.blockPrefab = blockPrefab;
            this.blockSpawner = blockSpawner;
            blockSpawner.BlockPrefabChanged += OnBlockPrefabChanged;
            blockName.text = blockPrefab.name;
        }

        protected void OnDestroy()
        {
            if (blockSpawner != null)
            {
                blockSpawner.BlockPrefabChanged -= OnBlockPrefabChanged;
            }
        }

        public void OnButtonClick()
        {
            blockSpawner.BlockPrefab = blockPrefab;
        }

        protected void OnBlockPrefabChanged(ChangeValueEventArgs<Block> e)
        {
            button.interactable = !(e.NewValue == blockPrefab);
        }
    }
}
