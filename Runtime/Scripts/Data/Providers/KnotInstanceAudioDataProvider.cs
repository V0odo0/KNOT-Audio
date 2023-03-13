using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Instance", order:-1000)]
    public class KnotInstanceAudioDataProvider : IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => _audioData ?? (_audioData = new KnotAudioData());
        [SerializeField] private KnotAudioData _audioData;
    }
}
