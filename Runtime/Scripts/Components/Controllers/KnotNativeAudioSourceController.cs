using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [DisallowMultipleComponent]
    public class KnotNativeAudioSourceController : KnotAudioSourceController
    {
        public override float MaxVolume
        {
            get => Mathf.Clamp01(_maxVolume);
            set => _maxVolume = value;
        }
        private float _maxVolume = 1;

        public override float PlayDelay
        {
            get => Mathf.Clamp(_playDelay, 0, float.MaxValue);
            set => _playDelay = value;
        }
        private float _playDelay;

        public override float TrimStart
        {
            get => Mathf.Clamp(_trimStart, 0, float.MaxValue);
            set => _trimStart = value;
        }
        private float _trimStart;

        public override float TrimEnd
        {
            get => Mathf.Clamp(_trimEnd, 0, float.MaxValue);
            set => _trimEnd = value;
        }
        private float _trimEnd = float.MaxValue;

        public override List<IKnotPlaybackBehaviour> PlaybackBehaviours =>
            _playbackBehaviours ?? (_playbackBehaviours = new List<IKnotPlaybackBehaviour>());

        [NonSerialized] private List<IKnotPlaybackBehaviour> _playbackBehaviours;
        [NonSerialized] private IKnotAudioData _audioData;
        [NonSerialized] private List<IKnotAudioMod> _allModsChain;

        private bool _destroyOnFinishPlaying;


        protected override void Awake()
        {
            base.Awake();

            AudioSource.playOnAwake = false;

            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.Awake);
        }

        protected virtual void Update()
        {
            UpdatePlayTime();

            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.Update);
        }

        protected virtual void FixedUpdate()
        {
            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.FixedUpdate);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.OnDestroy);
        }

        protected virtual void UpdatePlayTime()
        {
            if (AudioSource.loop)
            {
                if (AudioSource.time < TrimStart)
                    SetTimeForClip(TrimStart);
                else if (AudioSource.time >= TrimEnd)
                    SetTimeForClip(TrimStart);
            }
            else
            {
                if (_destroyOnFinishPlaying && !AudioSource.isPlaying || AudioSource.time >= TrimEnd)
                    Destroy(gameObject);
            }
        }

        protected virtual void SetTimeForClip(float t)
        {
            if (AudioSource.clip == null)
                return;

            t = Mathf.Clamp(t, 0, AudioSource.clip.length);
            AudioSource.time = t;
        }

        internal void UpdateAudioSourceName()
        {
            gameObject.name = $"{KnotAudio.CoreName}: {(AudioSource.clip == null ? "Empty" : AudioSource.clip.name)}";
        }

        
        public override KnotNativeAudioSourceController Initialize(IKnotAudioData audioData, params IKnotAudioMod[] mods)
        {
            if (audioData == null || audioData.AudioClip == null)
                return this;

            _audioData = audioData;
            AudioSource.playOnAwake = false;
            AudioSource.clip = audioData.AudioClip;
            
            if (_allModsChain == null)
                _allModsChain = new List<IKnotAudioMod>();
            else _allModsChain.Clear();

            var audioGroup = KnotAudio.GetAudioGroup(audioData.Group);
            if (audioGroup != null)
                _allModsChain.AddRange(audioGroup.Mods);
            _allModsChain.AddRange(audioData.GetAllMods());
            _allModsChain.AddRange(mods);
            
            foreach (var mod in _allModsChain)
                mod?.Initialize(this);

            UpdateAudioSourceName();

            return this;
        }

        public override KnotNativeAudioSourceController Play(bool loop)
        {
            _destroyOnFinishPlaying = !loop;
            AudioSource.loop = loop;

            SetTimeForClip(TrimStart);

            if (Mathf.Approximately(PlayDelay, 0))
                AudioSource.Play();
            else AudioSource.PlayDelayed(PlayDelay);

            return this;
        }
    }
}
