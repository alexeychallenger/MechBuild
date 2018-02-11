using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BlockCluster : MonoBehaviour
    {
        public List<Block> attachedBlockList;

        private void Start()
        {
            if (attachedBlockList != null)
            {
                foreach(Block block in attachedBlockList)
                {
                    RegisterBlock(block);
                }
            }
        }

        public void Init()
        {
            attachedBlockList = new List<Block>();
        }

        public void AddBlock(Block block)
        {
            attachedBlockList.Add(block);
            RegisterBlock(block);
        }

        private void RegisterBlock(Block block)
        {
            block.transform.SetParent(transform);
            block.blockCluster = this;
            block.BlockDestroyed += RemoveBlock;
        }

        public void RemoveBlock(Block block)
        {
            block.BlockDestroyed -= RemoveBlock;
            block.blockCluster = null;
            attachedBlockList.Remove(block);
        }
    }
}
