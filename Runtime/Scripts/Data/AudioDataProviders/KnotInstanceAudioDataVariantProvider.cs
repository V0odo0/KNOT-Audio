using System;
using Knot.Core;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Instance Variant", menuCustomName: "Variant/Instance")]
    public class KnotInstanceAudioDataVariantProvider : KnotVariantSelection<KnotInstanceAudioDataProvider>, IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext();
    }
}
