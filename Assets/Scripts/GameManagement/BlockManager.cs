using Assets.Scripts.Blocks;
using Assets.Scripts.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
    public class BlockManager : MonoBehaviour
    {
        public BlockSpawner blockSpawner;
        [HideInInspector] public List<Block> blockList = new List<Block>();
        [HideInInspector] public List<BlockCluster> clusterList = new List<BlockCluster>();
        [HideInInspector] public bool useGravity;

        protected void Awake()
        {
            Block.BlockCreated += AddBlock;
            Block.BlockDestroyed += RemoveBlock;
            BlockCluster.ClusterCreated += AddCluster;
            BlockCluster.ClusterCreated += SetClusterSettings;
            BlockCluster.ClusterDestroyed += RemoveCluster;
        }

        protected void OnDestroy()
        {
            Block.BlockCreated -= AddBlock;
            Block.BlockDestroyed -= RemoveBlock;
            BlockCluster.ClusterCreated -= AddCluster;
            BlockCluster.ClusterDestroyed -= RemoveCluster;
        }

        protected void AddBlock(Block block)
        {
            blockList.Add(block);
        }
        protected void RemoveBlock(Block block)
        {
            blockList.Remove(block);
        }

        protected void AddCluster(BlockCluster blockCluster)
        {
            clusterList.Add(blockCluster);
        }
        protected void RemoveCluster(BlockCluster blockCluster)
        {
            clusterList.Remove(blockCluster);
        }

        protected void SetClusterSettings(BlockCluster blockCluster)
        {
            blockCluster.SetSettings(useGravity);
        }

        public void SwitchGravity()
        {
            useGravity = !useGravity;
            foreach(BlockCluster blockCluster in clusterList)
            {
                blockCluster.GetComponent<Rigidbody>().useGravity = useGravity;
            }
            DbLog.Log(string.Format("Gravity switched to {0} state", useGravity), Color.blue, this);
        }
    }
}
