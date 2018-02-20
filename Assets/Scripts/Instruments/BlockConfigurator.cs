using Assets.Scripts.Blocks;
using Assets.Scripts.GameManagement;
using Assets.Scripts.UI.ModalWindows.Elements.BlockConfig;
using Assets.Scripts.UI.ModalWindows.Parameters;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Instruments
{
    public class BlockConfigurator : Instrument
    {
        public override InstrumentType Type
        {
            get
            {
                return InstrumentType.Configurator;
            }
        }

        public Color selectionColor;

        protected Block currentBlock;

        protected GameManager gameManager;

        protected void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

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

                    if (hit.collider.tag == Tags.block.ToString())
                    {
                        SelectBlock(hit.collider.GetComponent<Block>());
                    }
                    else if (hit.collider.tag == Tags.attachment.ToString())
                    {
                        SelectBlock(hit.collider.GetComponent<Attachment>().block);
                    }
                }
            }
        }

        private void SelectBlock(Block block)
        {
            ResetSelection();

            currentBlock = block;
            currentBlock.meshRendererComponent.material.color = selectionColor;

            gameManager.modalManager.CreateModal(BlockConfigDetector.GetBlockConfigModalParameters(block));
        }

        private void ResetSelection()
        {
            if (currentBlock != null)
            {
                currentBlock.meshRendererComponent.material.color = Color.white;
                currentBlock = null;
            }
        }

        public void OnDisable()
        {
            ResetSelection();
        }
    }
}
