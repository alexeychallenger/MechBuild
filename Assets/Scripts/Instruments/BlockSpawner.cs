using Assets.Scripts.Blocks;
using Assets.Scripts.Events;
using Assets.Scripts.GameManagement;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public class BlockSpawner : Instrument
    {
        protected Block blockPrefab;
        public event Action<ChangeValueEventArgs<Block>> BlockPrefabChanged;
        public Block BlockPrefab
        {
            get { return blockPrefab; }
            set
            {
                var oldVal = blockPrefab;
                if (blockPrefab != value)
                {
                    blockPrefab = value;
                    if (BlockPrefabChanged != null)
                    {
                        BlockPrefabChanged(new ChangeValueEventArgs<Block>(oldVal, blockPrefab));
                    }
                }
            }
        }

        protected bool enableBlockPreview;
        public event Action<ChangeValueEventArgs<bool>> EnableBlockPreviewValueChanged;
        public bool EnableBlockPreview
        {
            get { return enableBlockPreview; }
            set
            {
                var oldVal = enableBlockPreview;
                if (enableBlockPreview != value)
                {
                    enableBlockPreview = value;
                    if (EnableBlockPreviewValueChanged != null)
                    {
                        EnableBlockPreviewValueChanged(new ChangeValueEventArgs<bool>(oldVal, enableBlockPreview));
                    }
                }
            }
        }

        protected Block previewBlock;

        public BlockCluster blockClusterPrefab;
        public BlockManager blockManager;

        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Spawner;
            }
        }

        public float rotationAngle = 90f;
        protected Vector3 spawnRotation = new Vector3();
        protected int spawnBaseAttachmentIndex;

        protected void Awake()
        {
            BlockPrefabChanged += OnBlockPrefabChanged;
            EnableBlockPreviewValueChanged += OnEnableBlockPreviewValueChanged;
            EnableBlockPreview = true;
        }

        public void Update()
        {
            SpawnQuaternionInput();
            SpawnPositionInput();
            SpawnBlockInput();
            ShowBlockPreviewInput();
        }

        protected void OnDestroy()
        {
            BlockPrefabChanged -= OnBlockPrefabChanged;
            EnableBlockPreviewValueChanged -= OnEnableBlockPreviewValueChanged;
        }

        public void SpawnBlock(Attachment targetAttachment)
        {
            if (blockPrefab == null)
            {
                DbLog.LogWarning("Select block type first", this);
                return;
            }
            Block block = Instantiate(blockPrefab);
            block.Init(targetAttachment, spawnBaseAttachmentIndex, spawnRotation);
        }

        public void EnablePreviewBlock(Attachment targetAttachment)
        {
            if (previewBlock == null)
            {
                SpawnPreviewBlock();
                if (previewBlock == null) return;
            }
            previewBlock.gameObject.SetActive(true);
            previewBlock.ShowPreview(targetAttachment, spawnBaseAttachmentIndex, spawnRotation);
        }

        public void DisablePreviewBlock()
        {
            if (previewBlock == null) return;
            previewBlock.gameObject.SetActive(false);
        }

        protected void OnBlockPrefabChanged(ChangeValueEventArgs<Block> e)
        {
            DbLog.Log(string.Format("Block prefab changed from [{0}] to [{1}]", e.OldValue, e.NewValue), Color.magenta, this);
            spawnRotation = new Vector3();
            spawnBaseAttachmentIndex = 0;
            SpawnPreviewBlock();
        }

        protected void OnEnableBlockPreviewValueChanged(ChangeValueEventArgs<bool> e)
        {
            if (e.NewValue)
            {
                if (previewBlock == null) SpawnPreviewBlock();
            }
            else
            {
                if (previewBlock != null) Destroy(previewBlock.gameObject);
            }
            DbLog.Log(string.Format("Block preview {0}", e.NewValue ? "enabled" : "disabled"), Color.magenta, this);
        }

        protected void SpawnPreviewBlock()
        {
            if(blockPrefab == null) 
            {
                //DbLog.LogWarning("Block not selected for preview", this);
                return;
            }
            if (previewBlock != null) Destroy(previewBlock.gameObject);

            Block block = Instantiate(blockPrefab);
            block.InitPreview();
            block.gameObject.SetActive(false);
            previewBlock = block;
            spawnBaseAttachmentIndex = previewBlock.GetCurrentAttachmantIndex();
        }

        protected void SpawnBlockInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerManager.blockLayerMask))
                {
                    if (hit.collider.tag == Tags.attachment.ToString())
                    {
                        SpawnBlock(hit.collider.GetComponent<Attachment>());
                    }
                }
            }
        }

        protected void ShowBlockPreviewInput()
        {
            if (!enableBlockPreview)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerManager.blockLayerMask))
            {
                if (hit.collider.tag == Tags.attachment.ToString())
                {
                    Attachment attachment = hit.collider.GetComponent<Attachment>();
                    if (!attachment.block.isPreview)
                    {
                        EnablePreviewBlock(attachment);
                    }
                    return;
                }
            }
            DisablePreviewBlock();
        }
        
        protected void SpawnQuaternionInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                spawnRotation += new Vector3(0, 0, rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawnRotation += new Vector3(0, 0, -rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                spawnRotation += new Vector3(0, rotationAngle, 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                spawnRotation += new Vector3(0, -rotationAngle, 0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                spawnRotation += new Vector3(rotationAngle, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                spawnRotation += new Vector3(-rotationAngle, 0, 0);
            }
        }

        protected void SpawnPositionInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                spawnBaseAttachmentIndex = previewBlock.SwitchBaseAttachmentNext();
                DbLog.Log(string.Format("spawnBaseAttachment switched to {0}", spawnBaseAttachmentIndex), Color.yellow, this);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                spawnBaseAttachmentIndex = previewBlock.SwitchBaseAttachmentPrevious();
                DbLog.Log(string.Format("spawnBaseAttachment switched to {0}", spawnBaseAttachmentIndex), Color.yellow, this);
            }
        }
    }
}
