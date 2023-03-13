using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotAudioMod
    {
        void Initialize(KnotAudioSourceController sourceController);
    }
}
