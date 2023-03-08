using System;
using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    public class KnotAudioData : IKnotAudioData
    {
        public AudioClip AudioClip
        {
            get => _audioClip;
            set => _audioClip = value;
        }
        [SerializeField] private AudioClip _audioClip;

        public IList<IKnotAudioDataMod> Mods => _mods ?? (_mods = new List<IKnotAudioDataMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataMod), false)] private List<IKnotAudioDataMod> _mods;


        public KnotAudioData() { }

        public KnotAudioData(AudioClip audioClip, params IKnotAudioDataMod[] audioDataMods)
        {
            _audioClip = audioClip;

            foreach (var m in audioDataMods)
                Mods.Add(m);
        }


        public IEnumerable<IKnotAudioDataMod> GetAllMods() => Mods;
    }
}
