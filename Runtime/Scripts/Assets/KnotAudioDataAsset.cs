using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("Audio Data")]
    [CreateAssetMenu(fileName = "KnotAudioData", menuName = KnotAudio.CoreName + "/Audio Data", order = -1000)]
    public class KnotAudioDataAsset : ScriptableObject, IEquatable<KnotAudioData>
    {
        public KnotAudioData AudioData => _audioData;
        [SerializeField] private KnotAudioData _audioData;


        public bool Equals(KnotAudioData other)
        {
            return AudioData == other;
        }

        public static implicit operator KnotAudioData(KnotAudioDataAsset d) => d == null ? null : d.AudioData;
    }
}