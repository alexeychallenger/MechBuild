using Assets.Scripts.Events;
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
        public BlockType type;
        private BlockCluster blockCluster;
        public BlockCluster BlockCluster
        {
            get
            {
                return blockCluster;
            }
            set
            {
                var oldValue = blockCluster;
                if (value != oldValue)
                {
                    blockCluster = value;
                    if (BlockClusterChanged != null)
                    {
                        BlockClusterChanged(new ChangeValueEventArgs<BlockCluster>(oldValue, value));
                    }
                }
            }
        }
        public event Action<ChangeValueEventArgs<BlockCluster>> BlockClusterChanged;

        public bool UseGravity
        {
            get
            {
                return blockCluster.rigidbodyComponent.useGravity;
            }
            set
            {
                var oldValue = blockCluster.rigidbodyComponent.useGravity;
                if (value != oldValue)
                {
                    blockCluster.rigidbodyComponent.useGravity = value;
                    if (UseGravityValueChanged != null)
                    {
                        UseGravityValueChanged(new ChangeValueEventArgs<bool>(oldValue, value));
                    }
                }
            }
        }

        public bool IsFreeze
        {
            get
            {
                return blockCluster.rigidbodyComponent.isKinematic;
            }
            set
            {
                var oldValue = blockCluster.rigidbodyComponent.isKinematic;
                if (value != oldValue)
                {
                    blockCluster.rigidbodyComponent.isKinematic = value;
                    if (IsFreezeValueChanged != null)
                    {
                        IsFreezeValueChanged(new ChangeValueEventArgs<bool>(oldValue, value));
                    }
                }
            }
        }

        public List<Attachment> attachments;
        public List<Block> connectedBlocks;
        [Space]
        public MeshRenderer meshRendererComponent;
        public Material defaultMaterial;
        public Material previewMaterial;
        public Collider colliderComponent;

        public Attachment connectedAttachment;

        public event Action<ChangeValueEventArgs<float>> MassValueChanged;
        public event Action<ChangeValueEventArgs<bool>> UseGravityValueChanged;
        public event Action<ChangeValueEventArgs<bool>> IsFreezeValueChanged;

        [SerializeField] protected float mass = 1f;
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                value = Mathf.Clamp(value, 0, value);
                var oldValue = mass;
                if (value != oldValue)
                {
                    mass = value;
                    if (MassValueChanged != null)
                    {
                        MassValueChanged(new ChangeValueEventArgs<float>(oldValue, value));
                    }
                }
            }
        }

        
        [HideInInspector] public Attachment currentBaseAttachment;
        public static event Action<Block> BlockCreated;
        public static event Action<Block> BlockDestroyed;
        public event Action<Block> BlockInstanceDestroyed;

        public bool isPreview;

        public virtual void Attach(Block block)
        {
            ConnectBlock(block);
        }

        protected void Detach(Block block)
        {
            connectedBlocks.Remove(block);
        }

        protected virtual void Awake()
        {
            currentBaseAttachment = attachments[0];
        }

        protected virtual void Start()
        {
            if (BlockCluster == null && !isPreview)
            {
                RegisterBlockCluster(BlockCluster.SpawnCluster(transform.position));
            }
        }

        public virtual void Init(Attachment targetAttachment, int attachmentIndex, Vector3 rotation)
        {
            RegisterAttachment(targetAttachment);

            SwitchPreview(false);
            SetPosition(targetAttachment, attachmentIndex, rotation);

            RegisterBlockCluster(targetAttachment.block.BlockCluster);
            targetAttachment.block.Attach(this);

            OnBlockCreated(this);
        }

        protected virtual void RegisterAttachment(Attachment targetAttachment)
        {
            connectedAttachment = targetAttachment;
            connectedAttachment.AttachmentDestroyed += CleanAttachment;
        }

        protected virtual void CleanAttachment(Attachment attachment)
        {
            connectedAttachment.AttachmentDestroyed -= CleanAttachment;
            connectedAttachment = null;
        }

        protected void SwitchPreview(bool isPreview)
        {
            this.isPreview = isPreview;
            SwitchLayer(isPreview ? LayerType.Preview : LayerType.Block);
            name = string.Format("{0} {1} {2}", type, isPreview ? "(preview)" : "", gameObject.GetInstanceID());
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
            if (connectedBlocks.Contains(block)) return;

            connectedBlocks.Add(block);
            block.BlockInstanceDestroyed += Detach;
            block.Attach(this);
        }

        public virtual void RegisterBlockCluster(BlockCluster blockCluster)
        {
            if (BlockCluster == blockCluster) return;

            BlockCluster = blockCluster;
            BlockCluster.AddBlock(this);
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

        public int GetCurrentAttachmentIndex()
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

        public void DestroyBlock()
        {
            if (BlockDestroyed != null)
            {
                BlockDestroyed(this);
                DbLog.Log(string.Format("Block destroyed {0}", this), Color.green, this);
            }
            if (BlockInstanceDestroyed != null)
            {
                BlockInstanceDestroyed(this);
                DbLog.Log(string.Format("Block instance {0} destroyed", this), Color.green, this);
            }
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
        }
    }
}
