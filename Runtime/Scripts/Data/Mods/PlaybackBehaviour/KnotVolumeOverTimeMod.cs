using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Volume Over Time", menuCustomName: "Behaviour/Volume Over Time")]
    public class KnotVolumeOverTimeMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve VolumeOverTimeCurve
        {
            get => _volumeOverTimeCurve;
            set => _volumeOverTimeCurve = value;
        }
        [SerializeField] private AnimationCurve _volumeOverTimeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public KnotAudioClipTimeMode TimeMode
        {
            get => _timeMode;
            set => _timeMode = value;
        }
        [SerializeField] private KnotAudioClipTimeMode _timeMode = KnotAudioClipTimeMode.Normalized;


        float GetCurveTime(KnotAudioController controller)
        {
            if (TimeMode == KnotAudioClipTimeMode.AbsoluteSeconds || controller.AudioSource.clip == null)
                return VolumeOverTimeCurve.Evaluate(controller.AudioSource.time);

            return VolumeOverTimeCurve.Evaluate(Mathf.Clamp01(controller.AudioSource.time / controller.AudioSource.clip.length));
        }


        public void Setup(KnotAudioController controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioController controller) => this;

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioController controller)
        {
            if (VolumeOverTimeCurve == null)
                return;

            switch (behaviourEvent)
            {
                case KnotPlaybackBehaviourEvent.Update:
                    controller.AudioSource.volume = GetCurveTime(controller);
                    break;
            }
        }
    }
}
