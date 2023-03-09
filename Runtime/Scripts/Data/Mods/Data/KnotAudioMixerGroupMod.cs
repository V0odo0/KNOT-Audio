using System;
using Knot.Audio.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Audio Mixer Group", -960)]
    public class KnotAudioMixerGroupMod : IKnotAudioDataMod
    {
        public AudioMixerGroup Group
        {
            get => _group;
            set => _group = value;
        }
        [SerializeField] private AudioMixerGroup _group;


        public KnotAudioMixerGroupMod() { }

        public KnotAudioMixerGroupMod(AudioMixerGroup group)
        {
            _group = group;
        }


        public void Initialize(KnotAudioSource source)
        {
            source.AudioSource.outputAudioMixerGroup = _group;
        }
    }
}
