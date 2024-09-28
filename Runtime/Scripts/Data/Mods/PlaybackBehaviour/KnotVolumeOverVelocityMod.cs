using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Volume Over Velocity", menuCustomName: "Behaviour/Volume Over Velocity", order: 1000)]
    public class KnotVolumeOverVelocityMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve VolumeOverVelocityCurve
        {
            get => _volumeOverVelocityCurve;
            set => _volumeOverVelocityCurve = value;
        }
        [SerializeField] private AnimationCurve _volumeOverVelocityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float SmoothStep
        {
            get => Mathf.Clamp(_smoothStep, 0, float.MaxValue);
            set => _smoothStep = value;
        }
        [SerializeField, Min(0)] private float _smoothStep = Mathf.Infinity;


        private Vector3 _lastPos;
        private Quaternion _lastRot;


        public KnotVolumeOverVelocityMod() { }

        public KnotVolumeOverVelocityMod(AnimationCurve volumeOverVelocityCurve)
        {
            _volumeOverVelocityCurve = volumeOverVelocityCurve;
        }


        public void Setup(KnotAudioControllerBase controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller)
        {
            return new KnotVolumeOverVelocityMod(VolumeOverVelocityCurve);
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller)
        {
            if (VolumeOverVelocityCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Attach:
                case KnotPlaybackBehaviourEvent.Update:
                    float velocity = (controller.transform.position - _lastPos).magnitude;
                    var volume = VolumeOverVelocityCurve.Evaluate(velocity);
                    var targetVolume = Mathf.Clamp(volume, 0, controller.MaxVolume);

                    controller.AudioSource.volume = Mathf.Lerp(controller.AudioSource.volume, targetVolume, Time.deltaTime * SmoothStep);

                    _lastPos = controller.transform.position;
                    break;
            }
        }
    }
}
