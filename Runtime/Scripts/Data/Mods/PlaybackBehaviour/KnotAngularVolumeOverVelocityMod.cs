using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Angular Volume Over Velocity", menuCustomName: "Behaviour/Angular Volume Over Velocity", order: 1000)]
    public class KnotAngularVolumeOverVelocityMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve VolumeOverAngularVelocityCurve
        {
            get => _volumeOverAngularVelocityCurve;
            set => _volumeOverAngularVelocityCurve = value;
        }
        [SerializeField] private AnimationCurve _volumeOverAngularVelocityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float SmoothStep
        {
            get => Mathf.Clamp(_smoothStep, 0, float.MaxValue);
            set => _smoothStep = value;
        }
        [SerializeField, Min(0)] private float _smoothStep = Mathf.Infinity;


        private Quaternion _lastRot = Quaternion.identity;


        public KnotAngularVolumeOverVelocityMod() { }

        public KnotAngularVolumeOverVelocityMod(AnimationCurve volumeOverAngularVelocityCurve)
        {
            _volumeOverAngularVelocityCurve = volumeOverAngularVelocityCurve;
        }


        public void Setup(KnotAudioControllerBase controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller)
        {
            return new KnotAngularVolumeOverVelocityMod(VolumeOverAngularVelocityCurve)
            {
                SmoothStep = SmoothStep
            };
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller)
        {
            if (VolumeOverAngularVelocityCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Attach:
                case KnotPlaybackBehaviourEvent.Update:
                    float angularVelocity = Quaternion.Angle(controller.transform.rotation, _lastRot);
                    var volume = VolumeOverAngularVelocityCurve.Evaluate(angularVelocity);
                    var targetVolume = Mathf.Clamp(volume, 0, controller.MaxVolume);

                    controller.AudioSource.volume = Mathf.Lerp(controller.AudioSource.volume, targetVolume, Time.deltaTime * SmoothStep);

                    _lastRot = controller.transform.rotation;
                    break;
            }
        }
    }
}
