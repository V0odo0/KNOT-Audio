using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Knot.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [DisallowMultipleComponent]
    public class KnotNativeAudioController : KnotAudioController
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

        public override List<IKnotAudioMod> Mods => _mods ?? (_mods = new List<IKnotAudioMod>());
        private List<IKnotAudioMod> _mods = new List<IKnotAudioMod>();

        private IKnotAudioData _audioData;
        private List<IKnotPlaybackBehaviourMod> _attachedPlaybackBehaviours = new List<IKnotPlaybackBehaviourMod>();
        private KnotAudioPlayMode _playMode;
        private float _lastPlaybackTime;


        protected virtual void Start()
        {
            if (_audioData != null)
            {
                SetupMods();
                Play();
            }
        }

        protected virtual void Update()
        {
            UpdatePlayback();
            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.Update);
        }

        protected virtual void FixedUpdate()
        {
            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.FixedUpdate);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.Detach);
        }

        protected virtual void UpdatePlayback()
        {
            if (_playMode == KnotAudioPlayMode.OneShot)
            {
                Debug.Log($"{AudioSource.isPlaying}  {gameObject.name}");
                if (!AudioSource.isPlaying)
                    Destroy(gameObject);
            }
            else
            {
                if (AudioSource.pitch >= 0)
                {
                    if (_playMode == KnotAudioPlayMode.LoopSetupPerCycle && AudioSource.time < _lastPlaybackTime)
                        SetupMods();

                    if (AudioSource.time < TrimStart)
                        SetPlaybackTime(TrimStart);
                    else if (AudioSource.time >= TrimEnd)
                        SetPlaybackTime(TrimStart);
                }
                else
                {
                    if (_playMode == KnotAudioPlayMode.LoopSetupPerCycle && AudioSource.time > _lastPlaybackTime)
                        SetupMods();

                    if (AudioSource.time < TrimStart)
                        SetPlaybackTime(TrimEnd);
                    else if (AudioSource.time >= TrimEnd)
                        SetPlaybackTime(TrimEnd);
                }

                _lastPlaybackTime = AudioSource.time;
            }
        }

        protected virtual void SetPlaybackTime(float t)
        {
            if (AudioSource.clip == null)
                return;

            AudioSource.time = Mathf.Clamp(t, 0, AudioSource.clip.length);
            _lastPlaybackTime = AudioSource.time;
        }

        protected virtual void InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent behaviourEvent)
        {
            if (_attachedPlaybackBehaviours == null)
                return;

            foreach (var behaviour in _attachedPlaybackBehaviours)
                behaviour?.OnBehaviourStateEvent(behaviourEvent, this);
        }

        protected virtual void AttachPlaybackBehaviours(IEnumerable<IKnotPlaybackBehaviourMod> playbackBehaviourMods)
        {
            foreach (var pb in playbackBehaviourMods)
            {
                if (_attachedPlaybackBehaviours.Contains(pb))
                    continue;

                var instance = pb.GetInstance(this);
                instance.OnBehaviourStateEvent(KnotPlaybackBehaviourEvent.Attach, this);
            }
        }

        protected virtual void DetachPlaybackBehaviours(IEnumerable<IKnotPlaybackBehaviourMod> playbackBehaviourMods)
        {
            foreach (var pb in _attachedPlaybackBehaviours)
                pb.OnBehaviourStateEvent(KnotPlaybackBehaviourEvent.Detach, this);
        }

        protected virtual void Play()
        {
            AudioSource.Stop();
            AudioSource.loop = _playMode != KnotAudioPlayMode.OneShot;

            SetPlaybackTime(TrimStart);

            if (Mathf.Approximately(PlayDelay, 0))
                AudioSource.Play();
            else AudioSource.PlayDelayed(PlayDelay);
        }
        
        
        public override KnotAudioController Initialize(IKnotAudioData audioData, KnotAudioPlayMode playMode)
        {
            if (audioData == null || _audioData == audioData)
                return this;

            _audioData = audioData;
            _playMode = playMode;
            
            AudioSource.playOnAwake = false;
            AudioSource.clip = audioData.AudioClip;
            
            Mods.Clear();
            Mods.AddRange(KnotAudio.GetAudioGroupMods(audioData.GroupName));
            Mods.AddRange(audioData.GetAllMods());
            
            gameObject.name = $"{KnotAudio.CoreName}: {(AudioSource.clip == null ? "Empty" : AudioSource.clip.name)}";
            
            return this;
        }
        
        public override KnotAudioController SetupMods()
        {
            DetachPlaybackBehaviours(_attachedPlaybackBehaviours);
            _attachedPlaybackBehaviours.Clear();

            foreach (var mod in Mods)
                mod?.Setup(this);

            _attachedPlaybackBehaviours.AddRange(Mods.OfType<IKnotPlaybackBehaviourMod>());
            AttachPlaybackBehaviours(_attachedPlaybackBehaviours);

            return this;
        }

        public override KnotAudioController AppendMods(params IKnotAudioMod[] mods)
        {
            Mods.AddRange(mods.Where(m => m != null && !Mods.Contains(m)));
            return this;
        }
    }
}
