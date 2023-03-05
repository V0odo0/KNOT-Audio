using System;
using Knot.Audio.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Volume Range")]
    public class KnotVolumeRangeMod : IKnotAudioDataMod
    {
        public float Min
        {
            get => Mathf.Clamp(_min, 0, _max);
            set => _min = value;
        }
        [SerializeField, Range(0f, 1f)] private float _min = 1f;

        public float Max
        {
            get => Mathf.Clamp(_max, _min, 1);
            set => _max = value;
        }
        [SerializeField, Range(0f, 1f)] private float _max = 1f;


        public float Sample() => Random.Range(Min, Max);
    }
}
