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

        public HingeJoint hingeJointComponent;

        public override void Init(Attachment targetAttachment)
        {
            name = string.Format("{0} {1}", Type.ToString(), Guid.NewGuid());
            SetPosition(targetAttachment);
            blockCluster = BlockCluster.SpawnCluster(transform.position);
            blockCluster.AddBlock(this);
            AddHingeJointComponent(targetAttachment);
            targetAttachment.block.Attach(this);
            OnBlockCreated(this);
        }

        protected void AddHingeJointComponent(Attachment targetAttachment)
        {
            hingeJointComponent = blockCluster.gameObject.AddComponent<HingeJoint>();
            hingeJointComponent.connectedBody = targetAttachment.block.blockCluster.rigidbodyComponent;
            hingeJointComponent.axis = GetSpawnPointOffset().normalized;
        }
    }
}
