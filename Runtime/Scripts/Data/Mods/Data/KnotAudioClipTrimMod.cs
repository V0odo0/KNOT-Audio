using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("AudioClip Trim")]
    public class KnotAudioClipTrimMod : IKnotAudioDataMod
    {
        public float Start
        {
            get => Mathf.Clamp(_start, 0, _end);
            set => _start = value;
        }
        [SerializeField, Min(0f)] private float _start = 0;

        public float End
        {
            get => Mathf.Clamp(_end, _start, Mathf.Infinity);
            set => _end = value;
        }
        [SerializeField, Min(0f)] private float _end = Mathf.Infinity;

        public KnotAudioClipTimeMode TrimMode
        {
            get => _trimMode;
            set => _trimMode = value;
        }
        [SerializeField] private KnotAudioClipTimeMode _trimMode = KnotAudioClipTimeMode.AbsoluteSeconds;


        public KnotAudioClipTrimMod() { }

        public KnotAudioClipTrimMod(float start, float end, KnotAudioClipTimeMode trimMode = KnotAudioClipTimeMode.AbsoluteSeconds)
        {
            _start = start;
            _end = end;
            _trimMode = trimMode;
        }

        public KnotAudioClipTrimMod(float start, KnotAudioClipTimeMode trimMode = KnotAudioClipTimeMode.AbsoluteSeconds)
        {
            _start = start;
            _end = float.MaxValue;
            _trimMode = trimMode;
        }


        public void Initialize(KnotNativeAudioSourceController sourceController)
        {
            switch (TrimMode)
            {
                case KnotAudioClipTimeMode.AbsoluteSeconds:
                    sourceController.TrimStart = Start;
                    sourceController.TrimEnd = End;
                    break;
                case KnotAudioClipTimeMode.Normalized:
                    if (sourceController.AudioSource.clip != null)
                    {
                        sourceController.TrimStart = sourceController.AudioSource.clip.length * Mathf.Clamp01(Start);
                        sourceController.TrimEnd = sourceController.AudioSource.clip.length * Mathf.Clamp01(End);
                    }
                    break;
            }
        }
    }
}
