using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Library Entry Variant", menuCustomName: "Variant/Library Entry")]
    public class KnotLibraryEntryVariantProvider : KnotVariantSelection<KnotLibraryEntryProvider>, IKnotAudioDataProvider
    {
        public IKnotAudioData AudioData => SelectNext()?.AudioData;
    }
}
