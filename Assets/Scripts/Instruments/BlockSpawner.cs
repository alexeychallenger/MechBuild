using Assets.Scripts.Blocks;
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
        public Block blockPrefab;
        public BlockCluster blockClusterPrefab;
        public BlockManager blockManager;

        public bool isSpawnEnabled = true;

        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Spawner;
            }
        }

        public event Action<Block> BlockSpawned;

        public void SpawnBlock(Attachment targetAttachment)
        {
            if (!isSpawnEnabled) return;

            if (targetAttachment.block.blockCluster == null)
            {
                BlockCluster blockCluster = SpawnCluster();
                blockCluster.transform.position = targetAttachment.block.transform.position;
                blockCluster.AddBlock(targetAttachment.block);
            }
            Block block = Instantiate(blockPrefab);
            block.Init(targetAttachment);
        }

        public BlockCluster SpawnCluster()
        {
            BlockCluster blockCluster = Instantiate(blockClusterPrefab);
            blockCluster.Init();
            blockCluster.rigidbody.useGravity = blockManager.useGravity;
            return blockCluster;
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

        public void SwitchSpawnEnable()
        {
            isSpawnEnabled = !isSpawnEnabled;
            DbLog.Log(string.Format("Block spawn switched to {0} state", isSpawnEnabled), Color.blue, this);
        }
    }
}
