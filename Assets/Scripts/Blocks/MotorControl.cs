using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class MotorControl : MonoBehaviour
    {
        [SerializeField] protected float motorForce = 100f;
        [SerializeField] protected float motorVelocity = 100f;
        [SerializeField] protected float motorDamper = 100f;
        [SerializeField] protected bool freespin;
        [SerializeField] protected KeyCode forwardAxisKey = KeyCode.UpArrow;
        [SerializeField] protected KeyCode backAxisKey = KeyCode.DownArrow;
        [SerializeField] protected bool isReverse;

        public enum MotorState
        {
            DriveForward,
            DriveBack,
            DriveOff
        }

        public event Action<ChangeValueEventArgs<bool>> Reverced;
        public event Action<ChangeValueEventArgs<float>> MotorForceChanged;
        public event Action<ChangeValueEventArgs<float>> MotorVelocityChanged;
        public event Action<ChangeValueEventArgs<bool>> FreespinChanged;
        public event Action<ChangeValueEventArgs<KeyCode>> ForwardAxisKeyChanged;
        public event Action<ChangeValueEventArgs<KeyCode>> BackAxisKeyChanged;

        public bool IsReverse
        {
            get
            {
                return isReverse;
            }
            set
            {
                var oldValue = isReverse;
                if (value != oldValue)
                {
                    isReverse = value;
                    if (Reverced != null)
                    {
                        Reverced(new ChangeValueEventArgs<bool>(oldValue, value));
                    }
                }
            }
        }

        public float MotorForce
        {
            get
            {
                return motorForce;
            }

            set
            {
                var oldValue = motorForce;
                if (value != oldValue)
                {
                    motorForce = value;
                    if (MotorForceChanged != null)
                    {
                        MotorForceChanged(new ChangeValueEventArgs<float>(oldValue, value));
                    }
                }
            }
        }

        public float MotorVelocity
        {
            get
            {
                return motorVelocity;
            }

            set
            {
                var oldValue = motorVelocity;
                if (value != oldValue)
                {
                    motorVelocity = value;
                    if (MotorVelocityChanged != null)
                    {
                        MotorVelocityChanged(new ChangeValueEventArgs<float>(oldValue, value));
                    }
                }
            }
        }

        public bool Freespin
        {
            get
            {
                return freespin;
            }

            set
            {
                var oldValue = freespin;
                if (value != oldValue)
                {
                    freespin = value;
                    if (FreespinChanged != null)
                    {
                        FreespinChanged(new ChangeValueEventArgs<bool>(oldValue, value));
                    }
                }
            }
        }

        public KeyCode ForwardAxisKey
        {
            get
            {
                return forwardAxisKey;
            }

            set
            {
                var oldValue = forwardAxisKey;
                if (value != oldValue)
                {
                    forwardAxisKey = value;
                    if (ForwardAxisKeyChanged != null)
                    {
                        ForwardAxisKeyChanged(new ChangeValueEventArgs<KeyCode>(oldValue, value));
                    }
                }
            }
        }

        public KeyCode BackAxisKey
        {
            get
            {
                return backAxisKey;
            }

            set
            {
                var oldValue = backAxisKey;
                if (value != oldValue)
                {
                    backAxisKey = value;
                    if (BackAxisKeyChanged != null)
                    {
                        BackAxisKeyChanged(new ChangeValueEventArgs<KeyCode>(oldValue, value));
                    }
                }
            }
        }

        public MotorState state;

        public HingeBlock hingeBlock;
        public HingeJoint hingeJointComponent;

        protected void Start()
        {
            hingeJointComponent = hingeBlock.HingeJointComponent;
            hingeBlock.HingeJointComponentChanged += UpdateHingeJointComponent;
        }

        protected void UpdateHingeJointComponent(ChangeValueEventArgs<HingeJoint> e)
        {
            hingeJointComponent = e.NewValue;
        }

        protected void Update()
        {
            if (hingeJointComponent == null) return;

            MotorInput();
            MotorDrive();
        }

        protected void OnDestroy()
        {
            hingeBlock.HingeJointComponentChanged -= UpdateHingeJointComponent;
        }

        protected void MotorInput()
        {
            if (Input.GetKey(ForwardAxisKey))
            {
                state = MotorState.DriveForward;
            }
            else if (Input.GetKey(BackAxisKey))
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
                    hingeJointComponent.useSpring = true;
                    hingeJointComponent.spring = new JointSpring
                    {
                        spring = motorForce,
                        damper = motorDamper,
                        targetPosition = Vector3.Angle(transform.position, hingeJointComponent.connectedBody.position)
                    };

                    break;
                case MotorState.DriveForward:
                    hingeJointComponent.useMotor = true;
                    hingeJointComponent.useSpring = false;
                    hingeJointComponent.motor = new JointMotor
                    {
                        targetVelocity = MotorVelocity * (isReverse ? -1 : 1),
                        force = MotorForce,
                        freeSpin = Freespin
                    };
                    break;
                case MotorState.DriveBack:
                    hingeJointComponent.useMotor = true;
                    hingeJointComponent.useSpring = false;
                    hingeJointComponent.motor = new JointMotor
                    {
                        targetVelocity = -MotorVelocity * (isReverse ? -1 : 1),
                        force = MotorForce,
                        freeSpin = Freespin
                    };
                    break;
            }
        }
    }
}
