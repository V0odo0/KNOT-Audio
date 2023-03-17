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
        public override double InstanceCreationTimestamp => _instanceCreationTimestamp;
        private double _instanceCreationTimestamp;

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

        protected IEnumerable<IKnotAudioMod> AllMods => _setupMods.Union(_appendMods);

        private List<IKnotPlaybackBehaviourMod> _attachedPlaybackBehaviours = new List<IKnotPlaybackBehaviourMod>();
        private IKnotAudioData _audioData;
        private List<IKnotAudioMod> _setupMods = new List<IKnotAudioMod>();
        private List<IKnotAudioMod> _appendMods = new List<IKnotAudioMod>();
        private KnotAudioPlayMode _playMode;
        private float _lastLoopPlaybackTime;


        protected override void Awake()
        {
            base.Awake();

            AudioSource.playOnAwake = false;
            _instanceCreationTimestamp = Time.realtimeSinceStartupAsDouble;
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

            InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent.Detach);
        }

        protected virtual void UpdatePlayTime()
        {
            if (AudioSource.loop)
            {
                if (_playMode == KnotAudioPlayMode.Loop)
                {
                    if (AudioSource.time < TrimStart)
                        SetTimeForClip(TrimStart);
                    else if (AudioSource.time >= TrimEnd)
                        SetTimeForClip(TrimStart);
                }
                else if (_playMode == KnotAudioPlayMode.LoopSetupPerCycle)
                {
                    bool isLoopCycleEnded = false;
                    if (AudioSource.time < TrimStart)
                        SetTimeForClip(TrimStart);
                    else if (AudioSource.time >= TrimEnd)
                    {
                        isLoopCycleEnded = true;
                        SetTimeForClip(TrimStart);
                    }
                    else if (_lastLoopPlaybackTime > AudioSource.time)
                        isLoopCycleEnded = true;

                    if (isLoopCycleEnded)
                        Setup(_audioData, _setupMods);
                }

                _lastLoopPlaybackTime = AudioSource.time;
            }
            else
            {
                if (_playMode == KnotAudioPlayMode.OneShot && (!AudioSource.isPlaying || AudioSource.time >= TrimEnd))
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
                _attachedPlaybackBehaviours.Add(instance);
                instance.OnBehaviourStateEvent(KnotPlaybackBehaviourEvent.Attach, this);
            }
        }

        protected virtual void DetachPlaybackBehaviours(IEnumerable<IKnotPlaybackBehaviourMod> playbackBehaviourMods)
        {
            foreach (var pb in _attachedPlaybackBehaviours)
                pb.OnBehaviourStateEvent(KnotPlaybackBehaviourEvent.Detach, this);
        }

        internal void UpdateAudioSourceName()
        {
            gameObject.name = $"{KnotAudio.CoreName}: {(AudioSource.clip == null ? "Empty" : AudioSource.clip.name)}";
        }

        
        public override KnotAudioController Setup(IKnotAudioData audioData, IEnumerable<IKnotAudioMod> mods)
        {
            if (audioData == null || audioData.AudioClip == null)
                return this;

            _audioData = audioData;
            AudioSource.playOnAwake = false;
            AudioSource.clip = audioData.AudioClip;

            _setupMods.Clear();

            DetachPlaybackBehaviours(_attachedPlaybackBehaviours);
            _attachedPlaybackBehaviours.Clear();

            var audioGroup = KnotAudio.GetAudioGroup(audioData.Group);
            if (audioGroup != null)
                _setupMods.AddRange(audioGroup.Mods);
            _setupMods.AddRange(audioData.GetAllMods());
            _setupMods.AddRange(mods);
            
            foreach (var mod in AllMods)
                mod?.Setup(this);

            AttachPlaybackBehaviours(AllMods.OfType<IKnotPlaybackBehaviourMod>());
            UpdateAudioSourceName();

            return this;
        }

        public override KnotAudioController AppendMods(params IKnotAudioMod[] mods)
        {
            var prevClip = AudioSource.clip;

            _appendMods.AddRange(mods);
            foreach (var mod in _appendMods)
                mod?.Setup(this);

            AttachPlaybackBehaviours(mods.OfType<IKnotPlaybackBehaviourMod>());

            if (AudioSource.clip != prevClip)
                UpdateAudioSourceName();
            
            return this;
        }

        public override KnotAudioController Play(KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot)
        {
            _playMode = playMode;
            _lastLoopPlaybackTime = 0;

            AudioSource.loop = playMode != KnotAudioPlayMode.OneShot;

            SetTimeForClip(TrimStart);

            if (Mathf.Approximately(PlayDelay, 0))
                AudioSource.Play();
            else AudioSource.PlayDelayed(PlayDelay);

            return this;
        }
    }
}
