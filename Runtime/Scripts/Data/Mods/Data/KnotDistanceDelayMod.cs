using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    /// <summary>
    /// Delays playback by the time sound needs to travel from the controller to the <see cref="KnotAudio.AudioListener"/>.
    /// Reads the controller position during Setup, so place it after any mod that moves the controller (Set Parent, Set Position)
    /// </summary>
    [Serializable]
    [KnotTypeInfo(displayName: "Distance Delay", Order = -940)]
    public class KnotDistanceDelayMod : IKnotControllerMod, IKnotAudioGroupMod
    {
        public const float DefaultSpeedOfSound = 343f;

        /// <summary>
        /// Units per second
        /// </summary>
        public float SpeedOfSound
        {
            get => Mathf.Max(_speedOfSound, 0.001f);
            set => _speedOfSound = value;
        }
        [SerializeField, Min(0.001f)] private float _speedOfSound = DefaultSpeedOfSound;

        /// <summary>
        /// Adds the travel time to the delay set by previous mods instead of replacing it
        /// </summary>
        public bool Additive
        {
            get => _additive;
            set => _additive = value;
        }
        [SerializeField] private bool _additive = true;


        public KnotDistanceDelayMod() { }

        public KnotDistanceDelayMod(float speedOfSound, bool additive = true)
        {
            _speedOfSound = speedOfSound;
            _additive = additive;
        }


        public float GetDelay(KnotAudioControllerBase controller)
        {
            var listener = KnotAudio.AudioListener;
            if (controller == null || listener == null)
                return 0;

            return Vector3.Distance(listener.position, controller.transform.position) / SpeedOfSound;
        }

        public void Setup(KnotAudioControllerBase controller)
        {
            if (controller == null)
                return;

            var delay = GetDelay(controller);
            controller.PlayDelay = Additive ? controller.PlayDelay + delay : delay;
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithDistanceDelay(float speedOfSound = KnotDistanceDelayMod.DefaultSpeedOfSound, bool additive = true)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotDistanceDelayMod(speedOfSound, additive));

            return this;
        }
    }
}
