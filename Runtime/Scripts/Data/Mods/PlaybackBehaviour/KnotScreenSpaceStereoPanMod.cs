using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Screen Space Stereo Pan", menuCustomName: "Behaviour/Screen Space Stereo Pan", order:1000)]
    public class KnotScreenSpaceStereoPanMod : IKnotPlaybackBehaviourMod
    {
        public AnimationCurve PanOverScreenPosCurve
        {
            get => _panOverScreenPosCurve;
            set => _panOverScreenPosCurve = value;
        }
        [SerializeField] private AnimationCurve _panOverScreenPosCurve = new AnimationCurve(new Keyframe(-1, -1), new Keyframe(1, 1));


        public KnotScreenSpaceStereoPanMod() { }

        public KnotScreenSpaceStereoPanMod(AnimationCurve panOverScreenPosCurve)
        {
            _panOverScreenPosCurve = panOverScreenPosCurve;
        }


        public void Setup(KnotAudioControllerBase controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller)
        {
            return this;
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller)
        {
            if (Camera.main == null || PanOverScreenPosCurve == null)
                return;

            var screenPos = Camera.main.WorldToViewportPoint(controller.transform.position);
            controller.AudioSource.panStereo = PanOverScreenPosCurve.Evaluate(screenPos.x - 0.5f);
        }
    }
}
