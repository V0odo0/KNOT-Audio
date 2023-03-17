using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioController : KnotTrackedMonoBehaviour<KnotAudioController>
    {
        public abstract double InstanceCreationTimestamp { get; }

        public virtual AudioSource AudioSource =>
            (_audioSource ?? (_audioSource = GetComponent<AudioSource>())) ??
            gameObject.AddComponent<AudioSource>();
        private AudioSource _audioSource;

        public abstract float MaxVolume { get; set; }
        public abstract float PlayDelay { get; set; }
        public abstract float TrimStart { get; set; }
        public abstract float TrimEnd { get; set; }


        public abstract KnotAudioController Setup(IKnotAudioData audioData, IEnumerable<IKnotAudioMod> mods);

        public abstract KnotAudioController AppendMods(params IKnotAudioMod[] mods);

        public abstract KnotAudioController Play(KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot);
    }
}
