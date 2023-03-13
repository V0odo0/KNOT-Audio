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


        public static KnotNativeAudioSourceController SetParent(this KnotNativeAudioSourceController audioSourceController, Transform parent)
        {
            if (audioSourceController != null)
            {
                _setParent.Parent = parent;
                _setParent.Initialize(audioSourceController);
            }

            return audioSourceController;
        }

        public static KnotNativeAudioSourceController WithPitch(this KnotNativeAudioSourceController audioSourceController, float pitch)
        {
            if (audioSourceController != null)
            {
                _pitchRange.Min = pitch;
                _pitchRange.Max = pitch;
                _pitchRange.Initialize(audioSourceController);
            }

            return audioSourceController;
        }

        public static KnotNativeAudioSourceController WithPitchRange(this KnotNativeAudioSourceController audioSourceController, float min, float max)
        {
            if (audioSourceController != null)
            {
                _pitchRange.Min = min;
                _pitchRange.Max = max;
                _pitchRange.Initialize(audioSourceController);
            }

            return audioSourceController;
        }

        public static KnotNativeAudioSourceController WithVolume(this KnotNativeAudioSourceController audioSourceController, float volume)
        {
            if (audioSourceController != null)
            {
                _volumeRange.Min = volume;
                _volumeRange.Max = volume;
                _volumeRange.Initialize(audioSourceController);
            }

            return audioSourceController;
        }

        public static KnotNativeAudioSourceController WithVolumeRange(this KnotNativeAudioSourceController audioSourceController, float min, float max)
        {
            if (audioSourceController != null)
            {
                _volumeRange.Min = min;
                _volumeRange.Max = max;
                _volumeRange.Initialize(audioSourceController);
            }

            return audioSourceController;
        }

        public static KnotNativeAudioSourceController AtPosition(this KnotNativeAudioSourceController audioSourceController, Vector3 pos, Space space = Space.Self)
        {
            if (audioSourceController != null)
            {
                _setPosition.Position = pos;
                _setPosition.Space = space;
                _setPosition.Initialize(audioSourceController);
            }

            return audioSourceController;
        }
    }
}
