using Assets.Scripts.Blocks;
using Assets.Scripts.Events;
using Assets.Scripts.GameManagement;
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

        protected void Awake()
        {
            BlockPrefabChanged += OnBlockPrefabChanged;
            EnableBlockPreviewValueChanged += OnEnableBlockPreviewValueChanged;
            EnableBlockPreview = true;
        }

        protected void OnEnable()
        {
            if (enableBlockPreview)
            {
                Attachment.PointerEnter += Attachment_PointerEnter;
                Attachment.PointerExit += Attachment_PointerExit;
            }
        }

        protected void OnDisable()
        {
            Attachment.PointerEnter -= Attachment_PointerEnter;
            Attachment.PointerExit -= Attachment_PointerExit;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    if (hit.collider.tag == Tags.attachment.ToString())
                    {
                        SpawnBlock(hit.collider.GetComponent<Attachment>());
                    }
                }
            }
            ShowBlockPreview();
        }

        protected void OnDestroy()
        {
            OnDisable();
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
            block.Init(targetAttachment);
        }

        public void EnablePreviewBlock(Attachment targetAttachment)
        {
            if (previewBlock == null)
            {
                SpawnPreviewBlock();
                if (previewBlock == null) return;
            }
            previewBlock.gameObject.SetActive(true);
            previewBlock.ShowPreview(targetAttachment);
        }

        public void DisablePreviewBlock()
        {
            if (previewBlock == null)
                return;
            previewBlock.gameObject.SetActive(false);
        }

        protected void Attachment_PointerEnter(Attachment attachment)
        {
            DbLog.Log("enter");
            if (!enableBlockPreview) return;
            EnablePreviewBlock(attachment);
        }

        protected void Attachment_PointerExit(Attachment attachment)
        {
            DbLog.Log("exit");
            if (!enableBlockPreview) return;
            DisablePreviewBlock();
        }

        protected void OnBlockPrefabChanged(ChangeValueEventArgs<Block> e)
        {
            DbLog.Log(string.Format("Block prefab changed from [{0}] to [{1}]", e.OldValue, e.NewValue), Color.magenta, this);
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
                if (previewBlock != null) Destroy(previewBlock);
            }
            DbLog.Log(string.Format("Block preview {0}", e.NewValue ? "enabled" : "disabled"), Color.magenta, this);
        }

        protected Block SpawnPreviewBlock()
        {
            if(blockPrefab == null)
            {
                //DbLog.LogWarning("Block not selected for preview", this);
                return null;
            }
            if (previewBlock != null)
            {
                Destroy(previewBlock);
            }

            Block block = Instantiate(blockPrefab);
            block.InitPreview();
            block.gameObject.SetActive(false);
            previewBlock = block;
            return block;
        }

        public void ShowBlockPreview()
        {
            if (!enableBlockPreview)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
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
    }
}
