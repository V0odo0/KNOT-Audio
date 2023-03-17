using System;
using Knot.Audio.Attributes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Audio Listener Setup")]
    [MovedFrom(true, sourceClassName: "KnotAudioListenerConfigMod")]
    public class KnotAudioListenerSetupMod : IKnotAudioDataMod, IKnotAudioGroupMod
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


        public KnotAudioListenerSetupMod() { }

        public KnotAudioListenerSetupMod(bool ignoreListenerPause, bool ignoreListenerVolume)
        {
            _ignoreListenerPause = ignoreListenerPause;
            _ignoreListenerVolume = ignoreListenerVolume;
        }


        public void Setup(KnotAudioController controller)
        {
            controller.AudioSource.ignoreListenerPause = IgnoreListenerPause;
            controller.AudioSource.ignoreListenerVolume = IgnoreListenerVolume;
        }
    }
}
