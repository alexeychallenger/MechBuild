using Assets.Scripts.Events;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class MotorControl : MonoBehaviour
    {
        protected const float FIXABLE_FORCE = 100000f;

        [SerializeField] protected float motorForce = 100f;
        [SerializeField] protected float motorVelocity = 100f;
        [SerializeField] protected float motorDamper = 0f;
        [SerializeField] protected bool freespin;
        [SerializeField] protected KeyCode forwardAxisKey = KeyCode.UpArrow;
        [SerializeField] protected KeyCode backAxisKey = KeyCode.DownArrow;
        [SerializeField] protected bool isReverse;
        [SerializeField] protected bool isFixable = true;


        public enum MotorState
        {
            DriveForward,
            DriveBack,
            DriveOff
        }

        public event Action<ChangeValueEventArgs<bool>> Reverced;
        public event Action<ChangeValueEventArgs<float>> MotorForceChanged;
        public event Action<ChangeValueEventArgs<float>> MotorVelocityChanged;
        public event Action<ChangeValueEventArgs<float>> MotorDamperChanged;
        public event Action<ChangeValueEventArgs<bool>> FreespinChanged;
        public event Action<ChangeValueEventArgs<bool>> IsFixableValueChanged;
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

        public bool IsFixable
        {
            get
            {
                return isFixable;
            }
            set
            {
                var oldValue = isFixable;
                if (value != oldValue)
                {
                    isFixable = value;
                    if (IsFixableValueChanged != null)
                    {
                        IsFixableValueChanged(new ChangeValueEventArgs<bool>(oldValue, value));
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

        public float MotorDamper
        {
            get
            {
                return motorDamper;
            }

            set
            {
                var oldValue = motorDamper;
                if (value != oldValue)
                {
                    motorDamper = value;
                    if (MotorDamperChanged != null)
                    {
                        MotorDamperChanged(new ChangeValueEventArgs<float>(oldValue, value));
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
            
            Debug.DrawLine(hingeBlock.transform.position, hingeBlock.transform.position + hingeBlock.transform.forward * 10f, Color.red);
            Debug.DrawLine(hingeBlock.connectedAttachment.transform.position, hingeBlock.connectedAttachment.transform.position + hingeBlock.connectedAttachment.transform.forward * 10f, Color.blue);

            DbLog.LogFormat("Current angle: {0}", hingeJointComponent.spring.targetPosition);

            switch (state)
            {
                case MotorState.DriveOff:
                    CalculateAngle();
                    hingeJointComponent.useSpring = IsFixable;
                    hingeJointComponent.useMotor = false;

                    break;
                case MotorState.DriveForward:
                    isAngleCalculated = false;
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
                    isAngleCalculated = false;
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

        protected bool isAngleCalculated = false;

        private void CalculateAngle()
        {
            if (isAngleCalculated) return;

            hingeJointComponent.spring = new JointSpring
            {
                spring = FIXABLE_FORCE,
                damper = motorDamper,
                targetPosition = Vector3.SignedAngle
                        (
                            transform.InverseTransformDirection(hingeBlock.connectedAttachment.transform.forward),
                            transform.InverseTransformDirection(hingeBlock.transform.forward),
                            RoundUtils.AbsVector3(hingeJointComponent.axis)
                        )
            };
            DbLog.LogFormat("Blue vector: {0}", hingeBlock.connectedAttachment.transform.forward);
            DbLog.LogFormat("Red vector: {0}", hingeBlock.transform.forward);

            DbLog.LogFormat("New fixable angle: {0}", hingeJointComponent.spring.targetPosition);
            isAngleCalculated = true;
        }
    }
}
