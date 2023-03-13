using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Volume Curve")]
    public class KnotVolumeCurveBehaviour : IKnotPlaybackBehaviour
    {
        public AnimationCurve VolumeCurve
        {
            get => _volumeCurve;
            set => _volumeCurve = value;
        }
        [SerializeField] private AnimationCurve _volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public KnotAudioClipTimeMode TimeMode
        {
            get => _timeMode;
            set => _timeMode = value;
        }
        [SerializeField] private KnotAudioClipTimeMode _timeMode = KnotAudioClipTimeMode.Normalized;


        float GetCurveTime(KnotAudioSourceController sourceController)
        {
            if (TimeMode == KnotAudioClipTimeMode.AbsoluteSeconds || sourceController.AudioSource.clip == null)
                return VolumeCurve.Evaluate(sourceController.AudioSource.time);

            return VolumeCurve.Evaluate(Mathf.Clamp01(sourceController.AudioSource.time / sourceController.AudioSource.clip.length));
        }


        public IKnotPlaybackBehaviour GetInstance(KnotAudioSourceController sourceController) => this;

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioSourceController sourceController)
        {
            if (VolumeCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Awake:
                case KnotPlaybackBehaviourEvent.Update:
                    sourceController.AudioSource.volume = GetCurveTime(sourceController);
                    break;
            }
        }
    }
}
