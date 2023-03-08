using System;
using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
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

        public float BaseVolume { get; set; } = 1f;
        public float BasePitch { get; set; } = 1f;

        [NonSerialized]
        protected IKnotAudioData _audioData;


        public virtual void Initialize(IKnotAudioData audioData, params IKnotAudioMod[] mods)
        {
            if (audioData == null || audioData.AudioClip == null)
                return;

            _audioData = audioData;

            gameObject.name = audioData.AudioClip.name;

            AudioSource.clip = audioData.AudioClip;
            foreach (var mod in audioData.GetAllMods())
                mod?.Initialize(this);

            foreach (var mod in mods)
                mod?.Initialize(this);
        }

        public virtual void Play()
        {

        }
    }
}