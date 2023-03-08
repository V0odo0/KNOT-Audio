using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Asset")]
    public class KnotAssetAudioDataProvider : IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => _asset == null ? null : _asset.Data;
        [SerializeField] private KnotAudioDataAsset _asset;
    }
}
