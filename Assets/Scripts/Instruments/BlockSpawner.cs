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

        protected void Awake()
        {
            BlockPrefabChanged += OnBlockPrefabChanged;
        }

        protected void OnDestroy()
        {
            BlockPrefabChanged -= OnBlockPrefabChanged;
        }

        private void OnBlockPrefabChanged(ChangeValueEventArgs<Block> e)
        {
            DbLog.Log(string.Format("Block prefab changed from [{0}] to [{1}]", e.OldValue, e.NewValue), Color.magenta, this);
        }

        public event Action<ChangeValueEventArgs<Block>> BlockPrefabChanged;

        public BlockCluster blockClusterPrefab;
        public BlockManager blockManager;

        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Spawner;
            }
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

        public void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    if(hit.collider.tag == Tags.attachment.ToString())
                    {
                        SpawnBlock(hit.collider.GetComponent<Attachment>());
                    }
                }
            }
        }
    }
}
