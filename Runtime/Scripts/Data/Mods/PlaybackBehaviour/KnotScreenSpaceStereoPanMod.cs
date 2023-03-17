using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Screen Space Stereo Pan", menuCustomName: "Behaviour/Screen Space Stereo Pan")]
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


        public void Setup(KnotAudioController controller) { }

        public IKnotPlaybackBehaviourMod GetInstance(KnotAudioController controller)
        {
            return this;
        }

        public void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioController controller)
        {
            if (Camera.main == null || PanOverScreenPosCurve == null)
                return;

            var screenPos = Camera.main.WorldToViewportPoint(controller.transform.position);
            Debug.Log(screenPos);
        }
    }
}
