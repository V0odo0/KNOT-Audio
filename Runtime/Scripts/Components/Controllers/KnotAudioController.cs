using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioController : KnotTrackedMonoBehaviour<KnotAudioController>
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


        public abstract KnotAudioController Initialize(IKnotAudioData audioData, KnotAudioPlayMode playMode);

        public abstract KnotAudioController SetupMods();

        public abstract KnotAudioController AppendMods(params IKnotAudioMod[] mods);
    }
}
