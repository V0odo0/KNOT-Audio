using System;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"AudioClip Variant", Order = - 980)]
    public class KnotAudioClipVariantMod : KnotArrayVariantSelection<AudioClip>, IKnotAudioDataMod
    {
        public KnotAudioClipVariantMod() { }

        public KnotAudioClipVariantMod(SelectionMethod method, params AudioClip[] audioClips)
        {
            _method = method;
            _variants = new List<AudioClip>(audioClips);
        }


        public void Initialize(KnotAudioSource source)
        {
            source.AudioSource.clip = SelectNext();
        }
    }
}
