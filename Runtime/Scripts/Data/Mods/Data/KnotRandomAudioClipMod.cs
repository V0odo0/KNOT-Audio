using System;
using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Random Audio AudioClip", Order = - 980)]
    public class KnotRandomAudioClipMod : IKnotAudioDataMod
    {
        public IList<AudioClip> AudioClips => _audioClips ?? (_audioClips = new List<AudioClip>());
        [SerializeField] private List<AudioClip> _audioClips;


        public void Initialize(KnotAudioSource source)
        {

        }
    }
}
