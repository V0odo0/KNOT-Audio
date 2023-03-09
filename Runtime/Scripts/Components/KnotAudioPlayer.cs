using System;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/Audio Player", 1000)]
    public class KnotAudioPlayer : KnotTrackedMonoBehaviour<KnotAudioPlayer>
    {
        public virtual List<IKnotAudioDataProvider> AudioDataProviders
        {
            get => _audioDataProviders ?? (_audioDataProviders = new List<IKnotAudioDataProvider>());
            set
            {
                if (value == null)
                    _audioDataProviders?.Clear();
                else _audioDataProviders = value.ToList();
            }
        }
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataProvider))]
        private List<IKnotAudioDataProvider> _audioDataProviders;

        public virtual List<IKnotAudioSourceMod> Mods => _mods ?? (_mods = new List<IKnotAudioSourceMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioSourceMod), false)]
        private List<IKnotAudioSourceMod> _mods;

        public virtual bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }
        [SerializeField] private bool _playOnAwake;

        [NonSerialized] private KnotSetParentMod _setParentMod;
        [NonSerialized] private List<IKnotAudioMod> _audioSourceMods;


        protected override void Awake()
        {
            base.Awake();

            if (PlayOnAwake)
                Play();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected virtual void Reset()
        {
            _audioDataProviders = new List<IKnotAudioDataProvider>(new []{new KnotInstanceAudioDataProvider()});
        }


        public virtual void Play()
        {
            Play(0);
        }

        public virtual void Play(int id)
        {
            if (AudioDataProviders.Count == 0 || id < 0 || id >= AudioDataProviders.Count)
                return;

            _setParentMod ??= new KnotSetParentMod(transform);
            _audioSourceMods ??= new List<IKnotAudioMod>();
            _audioSourceMods.Clear();
            _audioSourceMods.Add(_setParentMod);
            _audioSourceMods.AddRange(Mods);

            KnotAudio.PlayOnce(AudioDataProviders[id], _audioSourceMods.ToArray());
        }
    }
}
