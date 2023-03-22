using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Bypass Config")]
    public class KnotBypassConfigMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public bool BypassEffects
        {
            get => _bypassEffects;
            set => _bypassEffects = value;
        }
        [SerializeField] private bool _bypassEffects;

        public bool BypassListenerEffects
        {
            get => _bypassListenerEffects;
            set => _bypassListenerEffects = value;
        }
        [SerializeField] private bool _bypassListenerEffects;

        public bool BypassReverbZones
        {
            get => _bypassReverbZones;
            set => _bypassReverbZones = value;
        }
        [SerializeField] private bool _bypassReverbZones;


        public KnotBypassConfigMod() { }

        public KnotBypassConfigMod(bool bypassEffects, bool bypassListenerEffects, bool bypassReverbZones)
        {
            _bypassEffects = bypassEffects;
            _bypassListenerEffects = bypassListenerEffects;
            _bypassReverbZones = bypassReverbZones;
        }


        public void Setup(KnotAudioControllerBase controller)
        {
            controller.AudioSource.bypassEffects = BypassEffects;
            controller.AudioSource.bypassListenerEffects = BypassListenerEffects;
            controller.AudioSource.bypassReverbZones = BypassReverbZones;

        }
    }
}
