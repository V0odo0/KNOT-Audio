using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Instance")]
    public class KnotInstanceAudioDataProvider : IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => _audioData ?? (_audioData = new KnotAudioData());
        [SerializeField] private KnotAudioData _audioData;
    }
}
