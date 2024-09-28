using System;
using Knot.Core;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("AudioListener Setup")]
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


        public void Setup(KnotAudioControllerBase controller)
        {
            controller.AudioSource.ignoreListenerPause = IgnoreListenerPause;
            controller.AudioSource.ignoreListenerVolume = IgnoreListenerVolume;
        }
    }
}
