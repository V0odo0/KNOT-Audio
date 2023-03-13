using System;
using Knot.Audio.Attributes;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Asset Variant")]
    public class KnotAssetVariantAudioDataProvider : KnotVariantSelection<KnotAudioDataAsset>, IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext()?.AudioData;
    }
}
