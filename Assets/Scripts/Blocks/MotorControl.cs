using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class MotorControl : MonoBehaviour
    {
        public float motorForce = 100f;
        public float motorVelocity = 100f;
        public bool freespin;
        public KeyCode forwardAxisKey = KeyCode.UpArrow;
        public KeyCode backAxisKey = KeyCode.DownArrow;

        public enum MotorState
        {
            DriveForward,
            DriveBack,
            DriveOff
        }

        public MotorState state;

        public HingeBlock hingeBlock;
        public HingeJoint hingeJointComponent;

        protected void Start()
        {
            hingeJointComponent = hingeBlock.hingeJointComponent;
        }

        protected void Update()
        {
            if (hingeJointComponent == null) return;

            MotorInput();
            MotorDrive();
        }

        protected void MotorInput()
        {
            if (Input.GetKey(forwardAxisKey))
            {
                state = MotorState.DriveForward;
            }
            else if (Input.GetKey(backAxisKey))
            {
                state = MotorState.DriveBack;
            }
            else
            {
                state = MotorState.DriveOff;
            }
        }

        protected void MotorDrive()
        {
            switch (state)
            {
                case MotorState.DriveOff:
                    hingeJointComponent.useMotor = false;
                    break;
                case MotorState.DriveForward:
                    hingeJointComponent.useMotor = true;
                    hingeJointComponent.motor = new JointMotor
                    {
                        targetVelocity = motorVelocity,
                        force = motorForce,
                        freeSpin = freespin
                    };
                    break;
                case MotorState.DriveBack:
                    hingeJointComponent.useMotor = true;
                    hingeJointComponent.motor = new JointMotor
                    {
                        targetVelocity = -motorVelocity,
                        force = motorForce,
                        freeSpin = freespin
                    };
                    break;
            }

        }
    }
}
