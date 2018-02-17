using Assets.Scripts.GameManagement;
using Assets.Scripts.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.InputControllers
{
    public class ConstructInput : MonoBehaviour
    {
        public BlockManager blockManager;
        public BlockSpawner blockSpawner;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                blockManager.SwitchGravity();
            }
        }
    }
}
