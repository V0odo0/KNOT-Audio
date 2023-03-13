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

        public List<IKnotAudioGroupMod> Mods => _mods ?? (_mods = new List<IKnotAudioGroupMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioGroupMod), false)] private List<IKnotAudioGroupMod> _mods;


        public KnotAudioGroup() { }

        public KnotAudioGroup(string name, params IKnotAudioGroupMod[] mods)
        {
            _name = name;
            Mods.AddRange(mods);
        }
    }
}
