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
    public class BlockDestroyer : Instrument
    {
        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Destroyer;
            }
        }

        public void Update()
        {
            BlockDestroyInput();
        }

        protected void BlockDestroyInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerManager.blockLayerMask))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;

                    if (hit.collider.tag == Tags.attachment.ToString())
                    {
                        DestroyBlock(hit.collider.GetComponent<Attachment>().block);
                    }
                    if (hit.collider.tag == Tags.block.ToString())
                    {
                        DestroyBlock(hit.collider.GetComponent<Block>());
                    }
                }
            }
        }

        protected void DestroyBlock(Block block)
        {
            block.DestroyBlock();
        }
    }
}
