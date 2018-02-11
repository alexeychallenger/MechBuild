using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public class BlockSpawner : MonoBehaviour
    {
        public Block blockPrefab;
        public BlockCluster blockClusterPrefab;

        public event Action<Block> BlockSpawned;

        public void SpawnBlock(Attachment target)
        {
            if (target.block.blockCluster == null)
            {
                BlockCluster blockCluster = SpawnCluster();
                blockCluster.AddBlock(target.block);
            }
            Block block = Instantiate(blockPrefab);
            block.name = string.Format("{0} {1}", block.Type.ToString(), Guid.NewGuid());
            block.transform.position = target.transform.position;
            block.transform.rotation = target.transform.rotation;
            block.transform.localPosition += block.GetSpawnPointOffcet();
            target.block.blockCluster.AddBlock(block);
            target.block.Attach(block);
        }

        public BlockCluster SpawnCluster()
        {
            BlockCluster blockCluster = Instantiate(blockClusterPrefab);
            blockCluster.name = string.Format("BlockCluster {0}", Guid.NewGuid());
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
    }
}
