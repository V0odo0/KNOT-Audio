using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioSourceController : KnotTrackedMonoBehaviour<KnotAudioSourceController>
    {
        public virtual AudioSource AudioSource =>
            (_audioSource ?? (_audioSource = GetComponent<AudioSource>())) ??
            gameObject.AddComponent<AudioSource>();
        private AudioSource _audioSource;

        public abstract float MaxVolume { get; set; }
        public abstract float PlayDelay { get; set; }
        public abstract float TrimStart { get; set; }
        public abstract float TrimEnd { get; set; }

        public abstract List<IKnotPlaybackBehaviour> PlaybackBehaviours { get; }


        protected virtual void InvokePlaybackBehavioursEvent(KnotPlaybackBehaviourEvent behaviourEvent)
        {
            if (PlaybackBehaviours == null)
                return;

            foreach (var behaviour in PlaybackBehaviours)
                behaviour?.OnBehaviourStateEvent(behaviourEvent, this);
        }

        public abstract KnotAudioSourceController Initialize(IKnotAudioData audioData,
            params IKnotAudioMod[] mods);

        public abstract KnotAudioSourceController Play(bool loop);
    }
}
