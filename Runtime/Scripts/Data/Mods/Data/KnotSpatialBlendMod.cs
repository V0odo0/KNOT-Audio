using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Spatial Blend", Order = -970)]
    public class KnotSpatialBlendMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public float Value
        {
            get => Mathf.Clamp01(_value);
            set => _value = value;
        }
        [SerializeField, Range(0f, 1f)] private float _value;


        public KnotSpatialBlendMod() { }

        public KnotSpatialBlendMod(float value)
        {
            _value = value;
        }


        public void Initialize(KnotNativeAudioSourceController sourceController)
        {
            sourceController.AudioSource.spatialBlend = Value;
        }
    }
}
