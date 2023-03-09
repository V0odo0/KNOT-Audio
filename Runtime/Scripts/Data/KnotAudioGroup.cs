using System;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    public class KnotAudioGroup
    {
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        [SerializeField] private string _name = "Default Audio Group";

        public List<IKnotAudioDataMod> Mods => _mods ?? (_mods = new List<IKnotAudioDataMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataMod), false)] private List<IKnotAudioDataMod> _mods;


        public KnotAudioGroup() { }

        public KnotAudioGroup(string name, params IKnotAudioDataMod[] mods)
        {
            _name = name;
            Mods.AddRange(mods);
        }
    }
}
