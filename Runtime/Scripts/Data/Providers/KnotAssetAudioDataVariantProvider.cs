using System;
using Knot.Audio.Attributes;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Asset Variant")]
    public class KnotAssetAudioDataVariantProvider : KnotArrayVariantSelection<KnotAudioDataAsset>, IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext()?.Data;
    }
}
