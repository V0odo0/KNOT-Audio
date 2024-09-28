using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Asset")]
    public class KnotAssetAudioDataProvider : IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => _asset == null ? null : _asset.AudioData;
        [SerializeField] private KnotAudioDataAsset _asset;
    }
}
