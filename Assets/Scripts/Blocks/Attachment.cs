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

        public static event Action<Attachment> PointerEnter;
        public static event Action<Attachment> PointerExit;

        public Vector3 GetPositionOffcet()
        {
            Vector3 offcet = transform.position - block.transform.position;
            return block.transform.localPosition + offcet;
        }

        public void OnPointerEnter()
        {
            if (PointerEnter != null)
            {
                PointerEnter(this);
            }
        }

        public void OnPointerExit()
        {
            if (PointerExit != null)
            {
                PointerExit(this);
            }
        }
    }
}
