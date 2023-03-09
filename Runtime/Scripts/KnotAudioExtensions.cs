using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public static class KnotAudioExtensions
    {
        private static KnotSetParentMod _setParent { get; } = new KnotSetParentMod();
        private static KnotVolumeRangeMod _volumeRange { get; } = new KnotVolumeRangeMod();
        private static KnotPitchRangeMod _pitchRange { get; } = new KnotPitchRangeMod();
        private static KnotSetPositionMod _setPosition { get; } = new KnotSetPositionMod();


        public static KnotAudioSource SetParent(this KnotAudioSource audioSource, Transform parent)
        {
            if (audioSource != null)
            {
                _setParent.Parent = parent;
                _setParent.Initialize(audioSource);
            }

            return audioSource;
        }

        public static KnotAudioSource WithPitch(this KnotAudioSource audioSource, float pitch)
        {
            if (audioSource != null)
            {
                _pitchRange.Min = pitch;
                _pitchRange.Max = pitch;
                _pitchRange.Initialize(audioSource);
            }

            return audioSource;
        }

        public static KnotAudioSource WithPitchRange(this KnotAudioSource audioSource, float min, float max)
        {
            if (audioSource != null)
            {
                _pitchRange.Min = min;
                _pitchRange.Max = max;
                _pitchRange.Initialize(audioSource);
            }

            return audioSource;
        }

        public static KnotAudioSource WithVolume(this KnotAudioSource audioSource, float volume)
        {
            if (audioSource != null)
            {
                _volumeRange.Min = volume;
                _volumeRange.Max = volume;
                _volumeRange.Initialize(audioSource);
            }

            return audioSource;
        }

        public static KnotAudioSource WithVolumeRange(this KnotAudioSource audioSource, float min, float max)
        {
            if (audioSource != null)
            {
                _volumeRange.Min = min;
                _volumeRange.Max = max;
                _volumeRange.Initialize(audioSource);
            }

            return audioSource;
        }

        public static KnotAudioSource AtPosition(this KnotAudioSource audioSource, Vector3 pos, Space space = Space.Self)
        {
            if (audioSource != null)
            {
                _setPosition.Position = pos;
                _setPosition.Space = space;
                _setPosition.Initialize(audioSource);
            }

            return audioSource;
        }
    }
}
