using System;
using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    public class KnotAudioData
    {
        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }
        [SerializeField] private AudioClip _clip;

        public IList<IKnotAudioDataMod> Mods => _mods ?? (_mods = new List<IKnotAudioDataMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataMod), false)] private List<IKnotAudioDataMod> _mods;

    }
}
