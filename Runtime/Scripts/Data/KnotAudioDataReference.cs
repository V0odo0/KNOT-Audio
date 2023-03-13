using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    public class KnotAudioDataReference
    {
        public IKnotAudioDataProvider Provider
        {
            get => _provider;
            set => _provider = value;
        }
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataProvider))] private IKnotAudioDataProvider _provider;
    }
}
