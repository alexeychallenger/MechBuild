using Assets.Scripts.Utils;
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
        [Space]
        public MeshRenderer meshRendererComponent;
        public Material defaultMaterial;
        public Material previewMaterial;
        public Collider colliderComponent;
        
        [HideInInspector] public Attachment currentBaseAttachment;
        public static event Action<Block> BlockCreated;
        public static event Action<Block> BlockDestroyed;
        public event Action<Block> BlockInstanceDestroyed;

        public bool isPreview;

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
            if (blockCluster == null && !isPreview)
            {
                BlockCluster blockCluster = BlockCluster.SpawnCluster(transform.position);
                blockCluster.AddBlock(this);
            }
        }

        public virtual void Init(Attachment targetAttachment)
        {
            SwitchPreview(false);

            SetPosition(targetAttachment);
            blockCluster = targetAttachment.block.blockCluster;
            targetAttachment.block.blockCluster.AddBlock(this);
            targetAttachment.block.Attach(this);

            OnBlockCreated(this);
        }

        private void SwitchPreview(bool isPreview)
        {
            this.isPreview = isPreview;
            SwitchLayer(isPreview ? LayerType.Preview : LayerType.Block);
            name = string.Format("{0} {1} {2}", Type, isPreview ? "(preview)" : "", gameObject.GetInstanceID());
            meshRendererComponent.material = isPreview ? previewMaterial : defaultMaterial;
            colliderComponent.isTrigger = isPreview;
        }

        private void SwitchLayer(LayerType layerType)
        {
            LayerManager.SwitchLayer(gameObject, layerType);
            foreach (Attachment attachent in attachments)
            {
                LayerManager.SwitchLayer(attachent.gameObject, layerType);
            }
        }

        public virtual void InitPreview()
        {
            SwitchPreview(true);
        }

        public virtual void ShowPreview(Attachment targetAttachment)
        {
            SetPosition(targetAttachment);
        }

        protected virtual void SetPosition(Attachment targetAttachment)
        {
            transform.position = targetAttachment.transform.position;
            transform.rotation = targetAttachment.transform.rotation;
            transform.localPosition += GetSpawnPointOffset();
        }

        protected void OnBlockCreated(Block block)
        {
            if (BlockCreated != null) BlockCreated(block);
        }

        protected void ConnectBlock(Block block)
        {
            connectedBlocks.Add(block);
            block.connectedBlocks.Add(this);
        }

        public Vector3 GetSpawnPointOffset()
        {
            Vector3 spawnPoint = currentBaseAttachment.transform.position - transform.position;
            return spawnPoint;
        }

        public void SwitchBaseAttachmentNext()
        {
            int currentAttachmentIndex = attachments.FindIndex((x) => x == currentBaseAttachment);
            int nextAttachmentIndex = (currentAttachmentIndex + 1) % attachments.Count();
            currentBaseAttachment = attachments[nextAttachmentIndex];
        }

        public void SwitchBaseAttachmentPrevious()
        {
            int currentAttachmentIndex = attachments.FindIndex((x) => x == currentBaseAttachment);
            int previousAttachmentIndex = (currentAttachmentIndex - 1);
            previousAttachmentIndex = previousAttachmentIndex < 0 ? previousAttachmentIndex : (attachments.Count - 1);
            currentBaseAttachment = attachments[previousAttachmentIndex];
        }

        public void SwitchBaseAttachment(Attachment attachment)
        {
            if (attachments.Contains(attachment))
            {
                currentBaseAttachment = attachment;
            }
            else
            {
                DbLog.LogError(string.Format("{0} don't has such attachment {1}", gameObject.name, attachment), this);
            }
        }

        protected void OnDestroy()
        {
            if (BlockDestroyed != null) BlockDestroyed(this);
            if (BlockInstanceDestroyed != null) BlockInstanceDestroyed(this);
        }
    }
}
