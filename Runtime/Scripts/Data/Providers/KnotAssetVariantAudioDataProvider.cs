using System;
using Knot.Audio.Attributes;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Asset Variant", menuCustomName:"Variant/Asset")]
    public class KnotAssetVariantAudioDataProvider : KnotVariantSelection<KnotAudioDataAsset>, IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext()?.AudioData;
    }
}
