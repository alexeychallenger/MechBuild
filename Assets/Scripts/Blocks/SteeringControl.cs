using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class SteeringControl : MotorControl
    {
        protected const float LIMIT_RANGE = 0.0001f;

        [SerializeField] protected float rotateSpeed = 1f;
        [SerializeField] protected float min = 1f;
        [SerializeField] protected float max = 1f;

        protected float currentAngle = 0f;

        private void Awake()
        {
            
        }

        protected override void Start()
        {
            base.Start();
            if (hingeBlock.HingeJointComponent == null)
            {
                hingeBlock.HingeJointComponentChanged += SetLimit;
            }
            else
            {
                hingeBlock.HingeJointComponent.useLimits = true;
                hingeBlock.HingeJointComponent.limits = new JointLimits
                {
                    min = currentAngle,
                    max = currentAngle + LIMIT_RANGE
                };
            }
        }


        private void SetLimit(ChangeValueEventArgs<HingeJoint> obj)
        {
            hingeJointComponent.useLimits = true;
            hingeJointComponent.limits = new JointLimits
            {
                min = currentAngle,
                max = currentAngle + LIMIT_RANGE
            };
        }

        protected override void MotorDrive()
        {
            float rotateVar;

            switch (state)
            {
                case MotorState.DriveOff:
                    break;
                case MotorState.DriveForward:
                    rotateVar = rotateSpeed * (isReverse ? -1 : 1);
                    currentAngle = Mathf.Clamp(currentAngle + rotateVar, min, max);
                    hingeJointComponent.limits = new JointLimits
                    {
                        min = currentAngle,
                        max = currentAngle + LIMIT_RANGE
                    };
                    break;
                case MotorState.DriveBack:
                    rotateVar = rotateSpeed * (isReverse ? -1 : 1);
                    currentAngle = Mathf.Clamp(currentAngle - rotateVar, min, max);
                    hingeJointComponent.limits = new JointLimits
                    {
                        min = currentAngle,
                        max = currentAngle + LIMIT_RANGE
                    };
                    break;
            }

            DbLog.LogFormat("Current angle: {0}", currentAngle);
        }
    }
}
