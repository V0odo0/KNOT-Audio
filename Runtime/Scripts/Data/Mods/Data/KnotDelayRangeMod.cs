using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Play Delay Range", Order = -950)]
    public class KnotDelayRangeMod : IKnotAudioDataMod
    {
        public float Min
        {
            get => Mathf.Clamp(_min, 0, _max);
            set => _min = value;
        }
        [SerializeField, Min(0)] private float _min = 0f;

        public float Max
        {
            get => Mathf.Clamp(_max, _min, float.MaxValue);
            set => _max = value;
        }
        [SerializeField, Min(0)] private float _max = 1f;


        public KnotDelayRangeMod() { }

        public KnotDelayRangeMod(float min = 1, float max = 1)
        {
            _min = min;
            _max = max;
        }


        public float Sample() => UnityEngine.Random.Range(Min, Max);

        public void Initialize(KnotAudioSourceController sourceController)
        {
            if (sourceController == null)
                return;

            sourceController.PlayDelay = Sample();
        }
    }
}
