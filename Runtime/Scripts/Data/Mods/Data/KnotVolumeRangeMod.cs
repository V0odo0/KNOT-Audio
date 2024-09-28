using System;
using Knot.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Volume Range", Order = -1000)]
    public class KnotVolumeRangeMod : IKnotAudioDataMod, IKnotAudioGroupMod
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


        public KnotVolumeRangeMod()
        {
            _min = _max = 1;
        }

        public KnotVolumeRangeMod(float min = 1, float max = 1)
        {
            _min = min;
            _max = max;
        }


        public float Sample() => Random.Range(Min, Max);

        public void Setup(KnotAudioControllerBase controller)
        {
            if (controller == null)
                return;

            var sample = Sample();
            controller.MaxVolume = sample;
            controller.AudioSource.volume = sample;
        }
    }
    
    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithVolume(float volume)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotVolumeRangeMod(volume));

            return this;
        }

        public KnotAudioControllerHandle WithVolumeRange(float min, float max)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotVolumeRangeMod(min, max));

            return this;
        }
    }
}
