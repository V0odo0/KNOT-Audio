using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [DisallowMultipleComponent]
    public class KnotAudioSource : KnotTrackedMonoBehaviour<KnotAudioSource>
    {
        public virtual AudioSource AudioSource =>
            (_audioSource ?? (_audioSource = GetComponent<AudioSource>())) ??
            gameObject.AddComponent<AudioSource>();
        private AudioSource _audioSource;

        public float PlayTimeNormalized
        {
            get
            {
                if (AudioSource.clip == null)
                    return 0;

                return Mathf.Clamp01(AudioSource.time / AudioSource.clip.length);
            }
        }
        public float TrimStart { get; set; } = 0f;
        public float TrimEnd { get; set; } = float.MaxValue;


        protected IKnotAudioData AudioData => _audioData;
        [NonSerialized] private IKnotAudioData _audioData;
        [NonSerialized] private List<IKnotAudioMod> _allModsChain;

        private bool _destroyOnFinishPlaying;


        protected virtual void Update()
        {
            if (_destroyOnFinishPlaying && !AudioSource.loop)
            {
                if (!AudioSource.isPlaying || AudioSource.time >= TrimEnd)
                    Destroy(gameObject);
            }
        }


        public virtual KnotAudioSource Initialize(IKnotAudioData audioData, params IKnotAudioMod[] mods)
        {
            if (audioData == null || audioData.AudioClip == null)
                return this;

            _audioData = audioData;
            AudioSource.clip = audioData.AudioClip;
            
            if (_allModsChain == null)
                _allModsChain = new List<IKnotAudioMod>();
            else _allModsChain.Clear();

            if (!string.IsNullOrEmpty(audioData.Group) && KnotAudio.AudioGroups.ContainsKey(audioData.Group))
                _allModsChain.AddRange(KnotAudio.AudioGroups[audioData.Group].Mods);
            _allModsChain.AddRange(audioData.GetAllMods());
            _allModsChain.AddRange(mods);
            
            foreach (var mod in _allModsChain)
                mod?.Initialize(this);

            UpdateAudioSourceName();

            return this;
        }

        public virtual KnotAudioSource PlayOnce()
        {
            if (AudioSource.clip != null && TrimStart < AudioSource.clip.length)
            {
                if (!Mathf.Approximately(TrimStart, 0))
                    AudioSource.time = TrimStart;

                AudioSource.Play();
            }

            _destroyOnFinishPlaying = true;
            return this;
        }

        internal void UpdateAudioSourceName()
        {
            gameObject.name = $"{KnotAudio.CoreName}: {(AudioSource.clip == null ? "Empty" : AudioSource.clip.name)}";
        }
    }
}
