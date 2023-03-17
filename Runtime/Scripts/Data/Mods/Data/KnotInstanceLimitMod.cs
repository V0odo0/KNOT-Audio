using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Instance Limit")]
    public class KnotInstanceLimitMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public int InstanceLimit
        {
            get => Mathf.Clamp(_instanceLimit, 1, int.MaxValue);
            set => _instanceLimit = value;
        }
        [SerializeField, Min(1)] private int _instanceLimit = 10;

        public InstanceLimitSolveMethod LimitSolveMethod
        {
            get => _limitSolveMethod;
            set => _limitSolveMethod = value;
        }
        [SerializeField] private InstanceLimitSolveMethod _limitSolveMethod;


        public KnotInstanceLimitMod() { }

        public KnotInstanceLimitMod(int instanceLimit, InstanceLimitSolveMethod limitSolveMethod)
        {
            _instanceLimit = instanceLimit;
            _limitSolveMethod = limitSolveMethod;
        }


        public void Setup(KnotAudioController controller)
        {

        }


        public enum InstanceLimitSolveMethod
        {
            DestroyOld,
            DestroyMostDistant,
            DonNotPlayNew
        }
    }
}
