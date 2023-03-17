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

        public virtual List<IKnotControllerMod> ControllerMods => _controllerMods ?? (_controllerMods = new List<IKnotControllerMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotControllerMod), false)]
        private List<IKnotControllerMod> _controllerMods;

        public virtual bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }
        [SerializeField] private bool _playOnAwake;

        public virtual KnotAudioPlayMode PlayMode
        {
            get => _playMode;
            set => _playMode = value;
        }
        [SerializeField] private KnotAudioPlayMode _playMode;


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

            AudioDataProviders[id].Play(PlayMode, ControllerMods.ToArray());
        }
    }
}
