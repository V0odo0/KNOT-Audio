using System;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/Audio Player", 0)]
    public class KnotAudioPlayer : KnotTrackedMonoBehaviour<KnotAudioPlayer>
    {
        public virtual List<IKnotAudioDataProvider> AudioDataProviders =>
            _audioDataProviders ?? (_audioDataProviders = new List<IKnotAudioDataProvider>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataProvider))]
        private List<IKnotAudioDataProvider> _audioDataProviders;

        public virtual List<IKnotAudioSourceMod> AudioSourceMods => _audioSourceMods ?? (_audioSourceMods = new List<IKnotAudioSourceMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioSourceMod), false)]
        private List<IKnotAudioSourceMod> _audioSourceMods;

        public virtual bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }
        [SerializeField] private bool _playOnAwake;

        public virtual bool Loop
        {
            get => _loop;
            set => _loop = value;
        }
        [SerializeField] private bool _loop;


        protected override void Awake()
        {
            base.Awake();

            if (PlayOnAwake)
                Play();
        }


        public virtual void Play()
        {
            Play(0);
        }

        public virtual void Play(int id)
        {
            if (id < 0 || id >= AudioDataProviders.Count)
                return;

            if (Loop)
                AudioDataProviders[id].PlayLoop(AudioSourceMods.ToArray());
            else AudioDataProviders[id].PlayOnce(AudioSourceMods.ToArray());
        }
    }
}
