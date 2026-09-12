using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Angular Pitch Over Velocity", menuCustomName: "Behaviour/Angular Pitch Over Velocity", order: 1000)]
    public class KnotAngularPitchOverVelocityMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve PitchOverAngularVelocityCurve
        {
            get => _pitchOverAngularVelocityCurve;
            set => _pitchOverAngularVelocityCurve = value;
        }
        [SerializeField] private AnimationCurve _pitchOverAngularVelocityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float SmoothStep
        {
            get => Mathf.Clamp(_smoothStep, 0, float.MaxValue);
            set => _smoothStep = value;
        }
        [SerializeField, Min(0)] private float _smoothStep = Mathf.Infinity;


        private Quaternion _lastRot = Quaternion.identity;


        public KnotAngularPitchOverVelocityMod() { }

        public KnotAngularPitchOverVelocityMod(AnimationCurve pitchOverAngularVelocityCurve)
        {
            _pitchOverAngularVelocityCurve = pitchOverAngularVelocityCurve;
        }


        public void Setup(KnotAudioControllerBase controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller)
        {
            return new KnotAngularPitchOverVelocityMod(PitchOverAngularVelocityCurve)
            {
                SmoothStep = SmoothStep
            };
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller)
        {
            if (PitchOverAngularVelocityCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Attach:
                case KnotPlaybackBehaviourEvent.Update:
                    float angularVelocity = Quaternion.Angle(controller.transform.rotation, _lastRot);
                    var targetPitch = PitchOverAngularVelocityCurve.Evaluate(angularVelocity);

                    controller.AudioSource.pitch = Mathf.Lerp(controller.AudioSource.pitch, targetPitch, Time.deltaTime * SmoothStep);

                    _lastRot = controller.transform.rotation;
                    break;
            }
        }
    }
}
