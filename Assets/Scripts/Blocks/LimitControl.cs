using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class LimitControl : MonoBehaviour
    {
        [SerializeField] protected float minLimit = -75f;
        [SerializeField] protected float maxLimit = 75f;
        [SerializeField] protected bool useLimits = true;
        

        public float MinLimit 
        {
            get
            {
                return minLimit;
            }
            set
            {
                var oldValue = minLimit;
                if (value != oldValue)
                {
                    minLimit = value;
                    SetHingeJointLimits();
                    if (MinLimitChanged != null)
                    {
                        MinLimitChanged(new ChangeValueEventArgs<float>(oldValue, value));
                    }
                }
            }
        }
        public float MaxLimit
        {
            get
            {
                return maxLimit;
            }
            set
            {
                var oldValue = maxLimit;
                if (value != oldValue)
                {
                    maxLimit = value;
                    SetHingeJointLimits();
                    if (MaxLimitChanged != null)
                    {
                        MaxLimitChanged(new ChangeValueEventArgs<float>(oldValue, value));
                    }
                }
            }
        }
        public bool UseLimits
        {
            get
            {
                return useLimits;
            }
            set
            {
                var oldValue = useLimits;
                if (value != oldValue)
                {
                    useLimits = value;
                    SetHingeJointLimits();
                    if (UseLimitsChanged != null)
                    {
                        UseLimitsChanged(new ChangeValueEventArgs<bool>(oldValue, value));
                    }
                }
            }
        }

        public event Action<ChangeValueEventArgs<float>> MinLimitChanged;
        public event Action<ChangeValueEventArgs<float>> MaxLimitChanged;
        public event Action<ChangeValueEventArgs<bool>> UseLimitsChanged;

        public HingeBlock hingeBlock;
        public HingeJoint hingeJointComponent;

        protected void Start()
        {
            hingeJointComponent = hingeBlock.HingeJointComponent;
            hingeBlock.HingeJointComponentChanged += UpdateHingeJointComponent;
            SetHingeJointLimits();
        }

        protected void UpdateHingeJointComponent(ChangeValueEventArgs<HingeJoint> e)
        {
            hingeJointComponent = e.NewValue;
        }

        protected void OnDestroy()
        {
            hingeBlock.HingeJointComponentChanged -= UpdateHingeJointComponent;
        }

        protected void SetHingeJointLimits()
        {
            if (hingeJointComponent == null) return;

            hingeJointComponent.useLimits = useLimits;
            hingeJointComponent.limits = new JointLimits
            {
                min = minLimit,
                max = maxLimit
            };
        }
    }
}
