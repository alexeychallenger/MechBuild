using Assets.Scripts.Blocks;
using Assets.Scripts.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Instrument
{
    public class BlockPanelController : MonoBehaviour
    {
        protected readonly string blockPrefabsPath = "Prefabs/Blocks";

        public BlockSelectButton blockSelectButtonPrefab;
        public BlockSpawner blockSpawner;

        protected void Start()
        {
            SpawnBlockSelectButtons();
        }

        protected void SpawnBlockSelectButtons()
        {
            Block[] blockArray = LoadBlockPrefabs();
            if (blockArray == null) return;
            foreach(Block block in blockArray)
            {
                BlockSelectButton blockSelectButton = Instantiate(blockSelectButtonPrefab, transform);
                blockSelectButton.Init(block, blockSpawner);
            }
        }

        protected Block[] LoadBlockPrefabs()
        {
            Block[] blockArray = Resources.LoadAll<Block>(blockPrefabsPath);
            if (blockArray == null || blockArray.Length == 0)
            {
                DbLog.LogWarning(string.Format("Block prefabs not found at path {0}", blockPrefabsPath), this);
            }
            return blockArray;
        }
    }
}
