using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Audio/" + KnotAudio.CoreName + "/Audio Source Area", 1000)]
    public class KnotAudioSourceArea : KnotAreaObject
    {
        public List<AudioSource> AudioSources => _audioSources ?? (_audioSources = new List<AudioSource>());
        [SerializeField] private List<AudioSource> _audioSources;

        public bool ControlSpread
        {
            get => _controlSpread;
            set => _controlSpread = value;
        }
        [SerializeField] protected bool _controlSpread = true;

        public AnimationCurve SpreadBlendCurve
        {
            get => _spreadBlendCurve;
            set => _spreadBlendCurve = value;
        }
        [SerializeField] protected AnimationCurve _spreadBlendCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


        [NonSerialized]
        protected Dictionary<AudioSource, AudioSourceSettings> _attachedAudioSources =
            new Dictionary<AudioSource, AudioSourceSettings>();


        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var audioSource in AudioSources)
                Attach(audioSource);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (var audioSource in AudioSources)
                Detach(audioSource);
        }

        protected virtual void Update()
        {
            UpdateAudioSources();
        }

        protected virtual void UpdateAudioSources()
        {
            if (KnotAudio.AudioListener == null)
                return;

            var sample = SampleAllVolumes(KnotAudio.AudioListener.position, BlendDistance);
            foreach (var a in _attachedAudioSources)
            {
                if (a.Key == null)
                    continue;

                a.Key.transform.position = Mathf.Approximately(sample.weight, 1)
                    ? KnotAudio.AudioListener.position
                    : sample.closestPoint;

                if (ControlSpread)
                {
                    var spreadBlend = SpreadBlendCurve.Evaluate(sample.weight);
                    a.Key.spread = Mathf.Lerp(a.Value.BaseSpread, 180, spreadBlend);
                }
            }
        }


        public virtual void Attach(AudioSource audioSource)
        {
            if (audioSource == null || _attachedAudioSources.ContainsKey(audioSource))
                return;

            _attachedAudioSources.Add(audioSource, new AudioSourceSettings(audioSource));
        }

        public virtual void Detach(AudioSource audioSource)
        {
            if (audioSource != null && _attachedAudioSources.ContainsKey(audioSource))
            {
                if (_controlSpread)
                    audioSource.spread = _attachedAudioSources[audioSource].BaseSpread;
                _attachedAudioSources.Remove(audioSource);
            }
        }

        protected class AudioSourceSettings
        {
            public readonly float BaseSpread;


            public AudioSourceSettings(AudioSource audioSource)
            {
                if (audioSource != null)
                    BaseSpread = audioSource.spread;
            }
        }
    }
}
