using Assets.Scripts.Blocks;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Instruments
{
    public class ClusterConnector : Instrument
    {
        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Connector;
            }
        }

        protected BlockCluster firstCluster;
        protected BlockCluster secondCluster;

        public Color highlightColor = Color.blue;


        protected void Update()
        {
            SelectInput();
        }

        protected void SelectInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerManager.blockLayerMask))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;

                    if (hit.collider.tag == Tags.attachment.ToString())
                    {
                        SelectCluster(hit.collider.GetComponent<Attachment>().block.BlockCluster);
                    }
                    if (hit.collider.tag == Tags.block.ToString())
                    {
                        SelectCluster(hit.collider.GetComponent<Block>().BlockCluster);
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                ResetSelection();
            }
        }

        protected void SelectCluster(BlockCluster blockCluster)
        {
            if (firstCluster == null)
            {
                firstCluster = blockCluster;
                ClusterHighlight(blockCluster, true);
                return;
            }
            if (secondCluster == null)
            {
                if (firstCluster == blockCluster) return;

                secondCluster = blockCluster;
                ClusterHighlight(blockCluster, true);
                ConnectClusters();
            }
        }

        protected void ResetSelection()
        {
            ClusterHighlight(firstCluster, false);
            firstCluster = null;
            ClusterHighlight(secondCluster, false);
            secondCluster = null;
        }

        protected void ClusterHighlight(BlockCluster blockCluster, bool state)
        {
            if (blockCluster == null) return;

            foreach (var block in blockCluster.attachedBlockList)
            {
                var material = block.SwitchMaterial(state);
            } 
        }

        protected void ConnectClusters()
        {
            List<Block> blockList = new List<Block>();
            Quaternion clusterRotation = firstCluster.transform.rotation;

            blockList.AddRange(firstCluster.attachedBlockList);
            firstCluster.DetachAllBlocks();
            blockList.AddRange(secondCluster.attachedBlockList);
            secondCluster.DetachAllBlocks();

            List<Vector3> blockPositions = new List<Vector3>();
            foreach (var block in blockList)
            {
                blockPositions.Add(block.transform.position);
            }
            Vector3 clusterPosition = VectorUtils.FindCentroid(blockPositions);

            BlockCluster blockCluster = BlockCluster.SpawnCluster(clusterPosition);
            foreach (var block in blockList)
            {
                blockCluster.AddBlockRange(blockList);
            }

            ClusterHighlight(blockCluster, false);
            ResetSelection();
        }
    }
}
