using System;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"AudioClip Variant", Order = - 980)]
    public class KnotAudioClipVariantMod : KnotVariantSelection<AudioClip>, IKnotAudioDataMod
    {
        public KnotAudioClipVariantMod() { }

        public KnotAudioClipVariantMod(SelectionMethod method, params AudioClip[] audioClips)
        {
            _selection = method;
            _variants = new List<AudioClip>(audioClips);
        }


        public void Initialize(KnotNativeAudioSourceController sourceController)
        {
            sourceController.AudioSource.clip = SelectNext(sourceController.AudioSource.clip);
        }
    }
}
