using System;
using Knot.Audio.Attributes;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Instance Variant", menuCustomName: "Variant/Instance")]
    public class KnotInstanceAudioDataVariantProvider : KnotVariantSelection<KnotInstanceAudioDataProvider>, IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext();
    }
}
