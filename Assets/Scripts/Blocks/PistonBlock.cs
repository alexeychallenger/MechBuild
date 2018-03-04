using Assets.Scripts.Events;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class PistonBlock : Block
    {
        public Vector3 spawnRotation;
        public Vector3 spawnAnchor;
        public Vector3 connectionAnchor;


        public float springForce = 200f;

        protected Vector3 connectionAxis = Vector3.up;
        protected ConfigurableJoint configurableJoint;
        public event Action<ChangeValueEventArgs<ConfigurableJoint>> JointComponentChanged;

        public ConfigurableJoint ConfigurableJoint
        {
            get
            {
                return configurableJoint;
            }
            set
            {
                var oldValue = configurableJoint;
                if (value != oldValue)
                {
                    configurableJoint = value;
                    if (JointComponentChanged != null)
                    {
                        JointComponentChanged(new ChangeValueEventArgs<ConfigurableJoint>(oldValue, value));
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
            AddJointComponent(connectedAttachment);
        }

        protected void AddJointComponent(Attachment targetAttachment)
        {
            if (ConfigurableJoint != null)
            {
                Destroy(ConfigurableJoint);
            }

            if (targetAttachment == null) return;

            ConfigurableJoint = BlockCluster.gameObject.AddComponent<ConfigurableJoint>();
            ConfigurableJoint.connectedBody = targetAttachment.block.BlockCluster.rigidbodyComponent;
            ConfigurableJoint.anchor = GetSpawnAnchorPoint();
            ConfigurableJoint.axis = transform.TransformDirection(connectionAxis);
            ConfigurableJoint.xMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint.yMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint.zMotion = ConfigurableJointMotion.Limited;
            ConfigurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint.linearLimit = new SoftJointLimit
            {
                limit = 1
            };
            ConfigurableJoint.zDrive = new JointDrive
            {
                positionSpring = springForce
            };


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
                Destroy(ConfigurableJoint);
                return;
            }
            AddJointComponent(connectedAttachment);
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

        //protected override void SetPosition(Attachment targetAttachment, int attachmentIndex, Quaternion rotation)
        //{
        //    ResetPosition();
        //    SwitchBaseAttachment(attachmentIndex);

        //    transform.Translate(targetAttachment.transform.position, Space.World);
        //    transform.rotation *= rotation;

        //    //Vector3 rotationDirection = (transform.position - connectionAnchor).normalized;

        //    transform.rotation *= Quaternion.Euler(spawnRotation);
        //    transform.Translate(spawnAnchor);
        //}
    }
}
