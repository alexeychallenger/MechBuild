using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BlockCluster : MonoBehaviour
    {
        public Rigidbody rigidbodyComponent;
        public List<Block> attachedBlockList;
        public static event Action<BlockCluster> ClusterCreated;
        public static event Action<BlockCluster> ClusterDestroyed;

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

        public static BlockCluster SpawnCluster()
        {
            BlockCluster blockClusterPrefab = Resources.Load<BlockCluster>("Prefabs/Construct/BlockCluster");
            BlockCluster blockCluster = Instantiate(blockClusterPrefab);
            blockCluster.Init();
            return blockCluster;
        }

        public static BlockCluster SpawnCluster(Vector3 position)
        {
            BlockCluster blockCluster = SpawnCluster();
            blockCluster.transform.position = position;
            return blockCluster;
        }

        public void Init()
        {
            name = string.Format("BlockCluster {0}", Guid.NewGuid());
            attachedBlockList = new List<Block>();
            Block.BlockDestroyed += RemoveBlock;
            if (ClusterCreated != null) ClusterCreated(this);
        }
        
        public void SetSettings(bool useGravity)
        {
            rigidbodyComponent.useGravity = useGravity;
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
            block.BlockInstanceDestroyed += RemoveBlock;
        }

        public void RemoveBlock(Block block)
        {
            block.BlockInstanceDestroyed -= RemoveBlock;
            block.blockCluster = null;
            attachedBlockList.Remove(block);
        }
    }
}
