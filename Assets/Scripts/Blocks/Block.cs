using Assets.Scripts.Utils;
using System;
using System.Collections;
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

        public virtual void Init(Attachment targetAttachment, int attachmentIndex, Vector3 rotation)
        {
            SwitchPreview(false);
            SetPosition(targetAttachment, attachmentIndex, rotation);
            blockCluster = targetAttachment.block.blockCluster;
            targetAttachment.block.blockCluster.AddBlock(this);
            targetAttachment.block.Attach(this);

            OnBlockCreated(this);
        }

        protected void SwitchPreview(bool isPreview)
        {
            this.isPreview = isPreview;
            SwitchLayer(isPreview ? LayerType.Preview : LayerType.Block);
            name = string.Format("{0} {1} {2}", Type, isPreview ? "(preview)" : "", gameObject.GetInstanceID());
            meshRendererComponent.material = isPreview ? previewMaterial : defaultMaterial;
            colliderComponent.isTrigger = isPreview;
        }

        protected void SwitchLayer(LayerType layerType)
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

        public virtual void ShowPreview(Attachment targetAttachment, int attachmentIndex, Vector3 rotation)
        {
            SetPosition(targetAttachment, attachmentIndex, rotation);
        }

        protected virtual void SetPosition(Attachment targetAttachment, int attachmentIndex, Vector3 rotation)
        {
            ResetPosition();
            SwitchBaseAttachment(attachmentIndex);
            
            transform.Translate(targetAttachment.transform.position, Space.World);
            transform.Rotate(targetAttachment.transform.rotation.eulerAngles, Space.World);

            transform.Rotate(rotation);

            Vector3 rotationDirection = currentBaseAttachment.transform.localRotation.eulerAngles;
            Vector3 translateDirection = RoundUtils.AbsVector3(currentBaseAttachment.transform.localPosition);

            transform.Rotate(rotationDirection);
            transform.Translate(translateDirection);
            
        }

        protected void ResetPosition()
        {
            SwitchBaseAttachment(0);
            transform.position = Vector3.zero;
            transform.rotation = new Quaternion();
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
            DbLog.LogFormat("local offset is {0}", spawnPoint);
            return spawnPoint;
        }

        public int SwitchBaseAttachmentNext()
        {
            int currentAttachmentIndex = attachments.FindIndex((x) => x == currentBaseAttachment);
            int nextAttachmentIndex = (currentAttachmentIndex + 1) % attachments.Count();
            currentBaseAttachment = attachments[nextAttachmentIndex];
            return nextAttachmentIndex;
        }

        public int SwitchBaseAttachmentPrevious()
        {
            int currentAttachmentIndex = attachments.FindIndex((x) => x == currentBaseAttachment);
            int previousAttachmentIndex = (currentAttachmentIndex - 1);
            previousAttachmentIndex = (previousAttachmentIndex >= 0) ? previousAttachmentIndex : (attachments.Count - 1);
            currentBaseAttachment = attachments[previousAttachmentIndex];
            return previousAttachmentIndex;
        }

        public int GetCurrentAttachmantIndex()
        {
            int currentAttachmentIndex = attachments.FindIndex((x) => x == currentBaseAttachment);
            return currentAttachmentIndex;
        }

        public Attachment SwitchBaseAttachment(int attachmentIndex)
        {
            if (attachmentIndex < 0 || attachmentIndex > (attachments.Count - 1))
            {
                DbLog.LogError(string.Format("{0} attachmentIndex {1} is out of attachments range", gameObject.name, attachmentIndex), this);
            }
            else
            {
                currentBaseAttachment = attachments[attachmentIndex];
            }
            return currentBaseAttachment;
        }

        protected void OnDestroy()
        {
            if (BlockDestroyed != null) BlockDestroyed(this);
            if (BlockInstanceDestroyed != null) BlockInstanceDestroyed(this);
        }
    }
}
