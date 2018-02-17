using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class Block : MonoBehaviour
    {
        public virtual BlockType Type
        {
            get
            {
                return BlockType.Block;
            }
        }
        public BlockCluster blockCluster;
        public List<Attachment> attachments;
        public List<Block> connectedBlocks;
        public Attachment currentBaseAttachment;

        public static event Action<Block> BlockCreated;
        public static event Action<Block> BlockDestroyed;
        public event Action<Block> BlockInstanceDestroyed;

        public virtual void Attach(Block block)
        {
            ConnectBlock(block);
        }

        protected virtual void Awake()
        {
            currentBaseAttachment = attachments[0];
        }

        protected virtual void Start()
        {
            if (blockCluster == null)
            {
                BlockCluster blockCluster = BlockCluster.SpawnCluster(transform.position);
                blockCluster.AddBlock(this);
            }
        }

        public virtual void Init(Attachment targetAttachment)
        {
            name = string.Format("{0} {1}", Type.ToString(), Guid.NewGuid());
            transform.position = targetAttachment.transform.position;
            transform.rotation = targetAttachment.transform.rotation;
            transform.localPosition += GetSpawnPointOffcet();
            targetAttachment.block.blockCluster.AddBlock(this);
            targetAttachment.block.Attach(this);
            if (BlockCreated != null) BlockCreated(this);
        }

        protected void ConnectBlock(Block block)
        {
            connectedBlocks.Add(block);
            block.connectedBlocks.Add(this);
        }

        public Vector3 GetSpawnPointOffcet()
        {
            Vector3 spawnPoint = currentBaseAttachment.transform.position - transform.position;
            return spawnPoint;
        }

        protected void OnDestroy()
        {
            if (BlockDestroyed != null) BlockDestroyed(this);
            if (BlockInstanceDestroyed != null) BlockInstanceDestroyed(this);
        }
    }
}
