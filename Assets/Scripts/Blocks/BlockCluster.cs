using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BlockCluster : MonoBehaviour
    {
        public Rigidbody rigidbodyComponent;
        public List<Block> attachedBlockList;
        public static event Action<BlockCluster> ClusterCreated;
        public static event Action<BlockCluster> ClusterDestroyed;
        public event Action<ChangeValueEventArgs<BlockCluster>> ClusterDivided;

        public bool IsKinematic
        {
            get
            {
                return rigidbodyComponent.isKinematic;
            }
            set
            {
                rigidbodyComponent.isKinematic = value;
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
            name = string.Format("BlockCluster {0}", gameObject.GetInstanceID());
            attachedBlockList = new List<Block>();
            if (ClusterCreated != null) ClusterCreated(this);
        }
        
        public void SetSettings(bool useGravity)
        {
            rigidbodyComponent.useGravity = useGravity;
        }

        public void AddBlock(Block block)
        {
            if (attachedBlockList.Contains(block))
            {
                return;
            }
            attachedBlockList.Add(block);
            RegisterBlock(block);
            rigidbodyComponent.mass += block.Mass;
        }

        public void AddBlockRange(Block[] blockArray)
        {
            foreach (Block block in blockArray)
            {
                AddBlock(block);
            }
        }

        private void RegisterBlock(Block block)
        {
            block.transform.SetParent(transform);
            block.RegisterBlockCluster(this);

            block.MassValueChanged += UpdateBlockMass;
            block.BlockInstanceDestroyed += OnBlockDestroyed;
        }

        private void UpdateBlockMass(ChangeValueEventArgs<float> e)
        {
            var valueDelta = e.NewValue - e.OldValue;
            rigidbodyComponent.mass += valueDelta;
        }

        public void OnBlockDestroyed(Block block)
        {
            RemoveBlock(block);

            if (attachedBlockList.Count != 0)
            {
                RecalculateCluster();
            }
        }

        private void RemoveBlock(Block block)
        {
            DbLog.Log(string.Format("RemoveBlock {0}", block), Color.green, this);
            block.BlockInstanceDestroyed -= OnBlockDestroyed;
            block.MassValueChanged -= UpdateBlockMass;

            rigidbodyComponent.mass -= block.Mass;
            block.BlockCluster = null;
            attachedBlockList.Remove(block);

            if (attachedBlockList.Count == 0)
            {
                DbLog.Log(string.Format("AttachedBlocks is 0. Deleting cluster", block), Color.green, this);
                transform.DetachChildren();
                Destroy(gameObject);
            }
        }

        protected void RecalculateCluster()
        {
            DbLog.Log(string.Format("Recalculate cluster"), Color.green, this);
            List<Block> unsortedBlocks = new List<Block>(attachedBlockList);
            List<List<Block>> newClusterDataList = new List<List<Block>>();
            
            while (unsortedBlocks.Count > 0)
            {
                newClusterDataList.Add(CalculateCluster(unsortedBlocks));
            }

            if (newClusterDataList.Count != 1)
            {
                foreach (List<Block> cluserData in newClusterDataList)
                {
                    BlockCluster blockCluster = SpawnCluster(transform.position);
                    foreach (Block block in cluserData)
                    {
                        RemoveBlock(block);
                        block.RegisterBlockCluster(blockCluster);
                    }

                    DbLog.Log(string.Format("Create new cluster {0}", blockCluster), Color.green, this);
                }
            }
        }

        protected List<Block> CalculateCluster(List<Block> unsortedBlocks)
        {
            List<Block> cluserData = new List<Block>();
            Block randomBlock = unsortedBlocks[UnityEngine.Random.Range(0, unsortedBlocks.Count)];

            AddBlockToClusterData(unsortedBlocks, cluserData, randomBlock);
            return cluserData;
        }

        protected void AddBlockToClusterData(List<Block> unsortedBlocks, List<Block> cluserData, Block blockToCheck)
        {
            unsortedBlocks.Remove(blockToCheck);
            cluserData.Add(blockToCheck);

            foreach (Block block in blockToCheck.connectedBlocks)
            {
                if (unsortedBlocks.Contains(block))
                {
                    AddBlockToClusterData(unsortedBlocks, cluserData, block);
                }
            }
        }

        protected void OnDestroy()
        {
            if (ClusterDestroyed != null) 
            {
                ClusterDestroyed(this);
                DbLog.Log(string.Format("Cluster {0} destroyed", this), Color.green, this);
            }
        }
    }
}
