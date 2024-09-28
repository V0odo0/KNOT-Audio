using System;
using Knot.Core;
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

        public void Setup(KnotAudioControllerBase controller)
        {
            if (controller == null)
                return;

            controller.PlayDelay = Sample();
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithDelay(float delay)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotDelayRangeMod(delay));

            return this;
        }

        public KnotAudioControllerHandle WithDelayRange(float min = 1, float max = 1)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotDelayRangeMod(min, max));

            return this;
        }
    }
}
