using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
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


        public void Setup(KnotAudioControllerBase controller)
        {
            switch (TrimMode)
            {
                case KnotAudioClipTimeMode.AbsoluteSeconds:
                    controller.TrimStart = Start;
                    controller.TrimEnd = End;
                    break;
                case KnotAudioClipTimeMode.Normalized:
                    if (controller.AudioSource.clip != null)
                    {
                        controller.TrimStart = controller.AudioSource.clip.length * Mathf.Clamp01(Start);
                        controller.TrimEnd = controller.AudioSource.clip.length * Mathf.Clamp01(End);
                    }
                    break;
            }
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle Trim(float start, float end = float.MaxValue)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotAudioClipTrimMod(start, end));

            return this;
        }

        public KnotAudioControllerHandle TrimNormalized(float start, float end = float.MaxValue)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotAudioClipTrimMod(start, end, KnotAudioClipTimeMode.Normalized));

            return this;
        }
    }
}
