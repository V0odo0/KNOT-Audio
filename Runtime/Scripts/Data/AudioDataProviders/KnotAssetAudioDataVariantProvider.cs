using System;
using Knot.Audio.Attributes;
using UnityEngine.Scripting.APIUpdating;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Asset Variant", menuCustomName:"Variant/Asset")]
    [MovedFrom(true, sourceClassName: "KnotAssetVariantAudioDataProvider")]
    public class KnotAssetAudioDataVariantProvider : KnotVariantSelection<KnotAudioDataAsset>, IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext()?.AudioData;
    }
}
