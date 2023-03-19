using System;
using Knot.Audio.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Audio Mixer Group", -960)]
    public class KnotAudioMixerGroupMod : IKnotAudioDataMod, IKnotAudioGroupMod
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


        public void Setup(KnotAudioController controller)
        {
            controller.AudioSource.outputAudioMixerGroup = _group;
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithAudioMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotAudioMixerGroupMod(audioMixerGroup));

            return this;
        }
    }
}
