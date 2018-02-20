using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Blocks
{
    public class HingeBlock : Block
    {
        public HingeJoint hingeJointComponent;

        public override void Init(Attachment targetAttachment, int baseAttachmentIndex, Vector3 rotation)
        {
            SwitchPreview(false);

            SetPosition(targetAttachment, baseAttachmentIndex, rotation);
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
            hingeJointComponent.axis = GetSpawnPointOffset();
        }
    }
}
