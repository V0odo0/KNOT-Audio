using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Pitch Range", Order = -990)]
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


        public KnotPitchRangeMod() { }

        public KnotPitchRangeMod(float min = 1, float max = 1)
        {
            _min = min;
            _max = max;
        }


        public float Sample() => UnityEngine.Random.Range(Min, Max);

        public void Initialize(KnotAudioSourceController sourceController)
        {
            if (sourceController == null)
                return;

            sourceController.AudioSource.pitch = Sample();
        }
    }
}
