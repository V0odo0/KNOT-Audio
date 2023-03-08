using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotAudioDataProvider
    {
        IKnotAudioData AudioData { get; }
    }
}
