using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Pitch Over Velocity", menuCustomName: "Behaviour/Pitch Over Velocity", order: 1000)]
    public class KnotPitchOverVelocityMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve PitchOverVelocityCurve
        {
            get => _pitchOverVelocityCurve;
            set => _pitchOverVelocityCurve = value;
        }
        [SerializeField] private AnimationCurve _pitchOverVelocityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float SmoothStep
        {
            get => Mathf.Clamp(_smoothStep, 0, float.MaxValue);
            set => _smoothStep = value;
        }
        [SerializeField, Min(0)] private float _smoothStep = Mathf.Infinity;


        private Vector3 _lastPos;


        public KnotPitchOverVelocityMod() { }

        public KnotPitchOverVelocityMod(AnimationCurve pitchOverVelocityCurve)
        {
            _pitchOverVelocityCurve = pitchOverVelocityCurve;
        }


        public void Setup(KnotAudioControllerBase controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller)
        {
            return new KnotPitchOverVelocityMod(PitchOverVelocityCurve)
            {
                SmoothStep = SmoothStep
            };
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller)
        {
            if (PitchOverVelocityCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Attach:
                case KnotPlaybackBehaviourEvent.Update:
                    float velocity = (controller.transform.position - _lastPos).magnitude;
                    var targetPitch = PitchOverVelocityCurve.Evaluate(velocity);

                    controller.AudioSource.pitch = Mathf.Lerp(controller.AudioSource.pitch, targetPitch, Time.deltaTime * SmoothStep);

                    _lastPos = controller.transform.position;
                    break;
            }
        }
    }
}
