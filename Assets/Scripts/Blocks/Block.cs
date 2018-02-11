using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        public abstract BlockType Type { get; }
        public BlockCluster blockCluster;
        public List<Attachment> attachments;
        public List<Block> connectedBlocks;
        public Attachment currentBaseAttachment;

        public event Action<Block> BlockDestroyed;

        public abstract void Attach(Block block);

        protected void Awake()
        {
            currentBaseAttachment = attachments[0];
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
        }
    }
}
