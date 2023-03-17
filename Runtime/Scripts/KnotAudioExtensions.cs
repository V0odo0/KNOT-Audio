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


        public static KnotNativeAudioController SetParent(this KnotNativeAudioController audioController, Transform parent)
        {
            if (audioController != null)
            {
                _setParent.Parent = parent;
                _setParent.Setup(audioController);
            }

            return audioController;
        }

        public static KnotNativeAudioController WithPitch(this KnotNativeAudioController audioController, float pitch)
        {
            if (audioController != null)
            {
                _pitchRange.Min = pitch;
                _pitchRange.Max = pitch;
                _pitchRange.Setup(audioController);
            }

            return audioController;
        }

        public static KnotNativeAudioController WithPitchRange(this KnotNativeAudioController audioController, float min, float max)
        {
            if (audioController != null)
            {
                _pitchRange.Min = min;
                _pitchRange.Max = max;
                _pitchRange.Setup(audioController);
            }

            return audioController;
        }

        public static KnotNativeAudioController WithVolume(this KnotNativeAudioController audioController, float volume)
        {
            if (audioController != null)
            {
                _volumeRange.Min = volume;
                _volumeRange.Max = volume;
                _volumeRange.Setup(audioController);
            }

            return audioController;
        }

        public static KnotNativeAudioController WithVolumeRange(this KnotNativeAudioController audioController, float min, float max)
        {
            if (audioController != null)
            {
                _volumeRange.Min = min;
                _volumeRange.Max = max;
                _volumeRange.Setup(audioController);
            }

            return audioController;
        }

        public static KnotNativeAudioController AtPosition(this KnotNativeAudioController audioController, Vector3 pos, Space space = Space.Self)
        {
            if (audioController != null)
            {
                _setPosition.Position = pos;
                _setPosition.Space = space;
                _setPosition.Setup(audioController);
            }

            return audioController;
        }
    }
}
