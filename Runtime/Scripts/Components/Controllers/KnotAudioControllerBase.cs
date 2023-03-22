using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioControllerBase : KnotTrackedMonoBehaviour<KnotAudioControllerBase>
    {
        internal double InstanceCreationTimestamp { get; private set; }

        public virtual AudioSource AudioSource =>
            (_audioSource ?? (_audioSource = GetComponent<AudioSource>())) ??
            gameObject.AddComponent<AudioSource>();
        private AudioSource _audioSource;

        public abstract float MaxVolume { get; set; }
        public abstract float PlayDelay { get; set; }
        public abstract float TrimStart { get; set; }
        public abstract float TrimEnd { get; set; }

        public abstract List<IKnotAudioMod> Mods { get; }


        protected override void Awake()
        {
            base.Awake();

            InstanceCreationTimestamp = Time.realtimeSinceStartupAsDouble;
        }


        public abstract KnotAudioControllerBase InitAsInstance(IKnotAudioData audioData, KnotAudioPlayMode playMode);

        public abstract KnotAudioControllerBase SetupMods();

        public abstract KnotAudioControllerBase AppendMods(params IKnotAudioMod[] mods);

        public abstract KnotAudioControllerBase Play();

        public abstract KnotAudioControllerBase Pause();

        public abstract KnotAudioControllerBase UnPause();
        
        public abstract KnotAudioControllerBase Stop();

    }
}
