using System;
using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("Audio Data")]
    [CreateAssetMenu(fileName = "KnotAudioDataAsset", menuName = KnotAudio.CoreName + "/Audio Data", order = -1000)]
    public class KnotAudioDataAsset : ScriptableObject, IEquatable<KnotAudioData>
    {
        public KnotAudioData Data => _data;
        [SerializeField] private KnotAudioData _data;


        public bool Equals(KnotAudioData other)
        {
            return Data == other;
        }


        public static implicit operator KnotAudioData(KnotAudioDataAsset d) => d.Data;
    }
}