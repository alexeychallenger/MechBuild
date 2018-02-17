using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class HingeBlock : Block
    {
        public override BlockType Type
        {
            get
            {
                return BlockType.HingeBlock;
            }
        }

        public HingeJoint hingeJoint;

        public override void Init(Attachment targetAttachment)
        {
            name = string.Format("{0} {1}", Type.ToString(), Guid.NewGuid());

            transform.position = targetAttachment.transform.position;
            transform.rotation = targetAttachment.transform.rotation;
            transform.localPosition += GetSpawnPointOffset();
            blockCluster = BlockCluster.SpawnCluster(transform.position);
            blockCluster.AddBlock(this);
            AddHingeJointComponent(targetAttachment);
            targetAttachment.block.Attach(this);
            OnBlockCreated(this);
        }

        protected void AddHingeJointComponent(Attachment targetAttachment)
        {
            hingeJoint = blockCluster.gameObject.AddComponent<HingeJoint>();
            hingeJoint.connectedBody = targetAttachment.block.blockCluster.rigidbodyComponent;
            hingeJoint.axis = GetSpawnPointOffset().normalized;
        }
    }
}
