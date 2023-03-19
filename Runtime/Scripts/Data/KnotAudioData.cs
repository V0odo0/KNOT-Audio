using System;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

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

        public string GroupName
        {
            get => _groupName;
            set => _groupName = value;
        }
        [FormerlySerializedAs("_group")]
        [SerializeField, KnotAudioGroupNamePicker] private string _groupName;

        public IList<IKnotAudioDataMod> Mods => _mods ?? (_mods = new List<IKnotAudioDataMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataMod), false)] private List<IKnotAudioDataMod> _mods;


        public KnotAudioData() { }

        public KnotAudioData(AudioClip audioClip, params IKnotAudioDataMod[] audioDataMods)
        {
            _audioClip = audioClip;
            foreach (var m in audioDataMods)
                Mods.Add(m);
        }

        public KnotAudioData(AudioClip audioClip, string group, params IKnotAudioDataMod[] audioDataMods)
        {
            _audioClip = audioClip;
            _groupName = group;
            foreach (var m in audioDataMods)
                Mods.Add(m);
        }


        public IEnumerable<IKnotAudioDataMod> GetAllMods() => Mods;
    }
}
