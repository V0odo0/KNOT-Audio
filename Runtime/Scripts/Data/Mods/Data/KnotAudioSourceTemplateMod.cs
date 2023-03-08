using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Audio Source Template")]
    public class KnotAudioSourceTemplateMod : IKnotAudioDataMod
    {
        public AudioSource Template
        {
            get => _template;
            set => _template = value;
        }
        [SerializeField] private AudioSource _template;


        public KnotAudioSourceTemplateMod() { }

        public KnotAudioSourceTemplateMod(AudioSource template)
        {
            _template = template;
        }


        public void Initialize(KnotAudioSource source)
        {

        }
    }
}
