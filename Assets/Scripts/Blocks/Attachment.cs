using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class Attachment : MonoBehaviour
    {
        public Block block;

        public Vector3 GetPositionOffcet()
        {
            Vector3 offcet = transform.position - block.transform.position;
            return block.transform.localPosition + offcet;
        }
    }
}
