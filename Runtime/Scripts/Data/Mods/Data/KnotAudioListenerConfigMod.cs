using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Audio Listener Config")]
    public class KnotAudioListenerConfigMod : IKnotAudioDataMod
    {
        public bool IgnoreListenerPause
        {
            get => _ignoreListenerPause;
            set => _ignoreListenerPause = value;
        }
        [SerializeField] private bool _ignoreListenerPause;

        public bool IgnoreListenerVolume
        {
            get => _ignoreListenerVolume;
            set => _ignoreListenerVolume = value;
        }
        [SerializeField] private bool _ignoreListenerVolume;


        public KnotAudioListenerConfigMod() { }

        public KnotAudioListenerConfigMod(bool ignoreListenerPause, bool ignoreListenerVolume)
        {
            _ignoreListenerPause = ignoreListenerPause;
            _ignoreListenerVolume = ignoreListenerVolume;
        }


        public void Initialize(KnotAudioSource source)
        {
            source.AudioSource.ignoreListenerPause = IgnoreListenerPause;
            source.AudioSource.ignoreListenerVolume = IgnoreListenerVolume;
        }
    }
}
