using System;
using Knot.Audio.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Knot.Audio
{

    [Serializable]
    [KnotTypeInfo(displayName: "Pitch Range")]
    public class KnotPitchRangeMod : IKnotAudioDataMod
    {
        public float Min
        {
            get => Mathf.Clamp(_min, -3, _max);
            set => _min = value;
        }
        [SerializeField, Range(-3f, 3f)] private float _min = 1f;

        public float Max
        {
            get => Mathf.Clamp(_max, _min, 3);
            set => _max = value;
        }
        [SerializeField, Range(-3f, 3f)] private float _max = 1f;


        public float Sample() => Random.Range(Min, Max);
    }
}
