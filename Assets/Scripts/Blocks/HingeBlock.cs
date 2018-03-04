using Assets.Scripts.Events;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class HingeBlock : Block
    {
        public bool useAutomaticConnectionAnchor = true;
        public Vector3 connectionAnchor;
        public Vector3 connectionAxis;
        protected HingeJoint hingeJointComponent;
        public event Action<ChangeValueEventArgs<HingeJoint>> HingeJointComponentChanged;
        public HingeJoint HingeJointComponent
        {
            get
            {
                return hingeJointComponent;
            }
            set
            {
                var oldValue = hingeJointComponent;
                if (value != oldValue)
                {
                    hingeJointComponent = value;
                    if (HingeJointComponentChanged != null)
                    {
                        HingeJointComponentChanged(new ChangeValueEventArgs<HingeJoint>(oldValue, value));
                    }
                }
            }
        }

        public override void Init(Attachment targetAttachment, int baseAttachmentIndex, Vector3 rotation)
        {
            RegisterAttachment(targetAttachment);

            SwitchPreview(false);

            SetPosition(targetAttachment, baseAttachmentIndex, rotation);
            RegisterBlockCluster(BlockCluster.SpawnCluster(transform.position));
            targetAttachment.block.Attach(this);
            OnBlockCreated(this);
        }



        public override void RegisterBlockCluster(BlockCluster blockCluster)
        {
            base.RegisterBlockCluster(blockCluster);
            AddHingeJointComponent(connectedAttachment);
        }

        protected void AddHingeJointComponent(Attachment targetAttachment)
        {
            if (HingeJointComponent != null)
            {
                Destroy(HingeJointComponent);
            }

            if (targetAttachment == null) return;

            HingeJointComponent = BlockCluster.gameObject.AddComponent<HingeJoint>();
            HingeJointComponent.connectedBody = targetAttachment.block.BlockCluster.rigidbodyComponent;
            HingeJointComponent.anchor = useAutomaticConnectionAnchor ? GetSpawnPointOffset() : GetSpawnAnchorPoint();
            HingeJointComponent.axis = transform.TransformDirection(connectionAxis);
        }

        private Vector3 GetSpawnAnchorPoint()
        {
            Vector3 offset = transform.TransformPoint(connectionAnchor) - transform.position;
            DbLog.LogFormat("local offset is {0}", offset);
            return offset;
        }

        protected void UpdateAttachmentBlockCluster(ChangeValueEventArgs<BlockCluster> e)
        {
            if (e.NewValue == null)
            {
                Destroy(HingeJointComponent);
                return;
            }
            AddHingeJointComponent(connectedAttachment);
        }

        protected override void CleanAttachment(Attachment attachment)
        {
            attachment.block.BlockClusterChanged -= UpdateAttachmentBlockCluster;
            base.CleanAttachment(attachment);
        }

        protected override void RegisterAttachment(Attachment targetAttachment)
        {
            base.RegisterAttachment(targetAttachment);
            targetAttachment.block.BlockClusterChanged += UpdateAttachmentBlockCluster;
        }

        protected override void SetPosition(Attachment targetAttachment, int attachmentIndex, Vector3 rotation)
        {
            ResetPosition();
            SwitchBaseAttachment(attachmentIndex);

            transform.Translate(targetAttachment.transform.position, Space.World);
            transform.Rotate(targetAttachment.transform.rotation.eulerAngles, Space.World);
            transform.Rotate(rotation);

            Vector3 rotationDirection = (transform.position - connectionAnchor).normalized;

            transform.Rotate(rotationDirection);
            transform.Translate(-connectionAnchor);
        }
    }
}
