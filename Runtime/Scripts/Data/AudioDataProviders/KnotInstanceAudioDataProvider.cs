using System;
using Knot.Audio.Attributes;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Instance", order:-1000)]
    public class KnotInstanceAudioDataProvider : KnotAudioData, IKnotPersistentAudioDataProvider
    {
        public IKnotAudioData AudioData => this;
    }
}
