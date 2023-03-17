using System;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    public enum KnotAudioPlayMode
    {
        OneShot,
        Loop,
        LoopSetupPerCycle
    }
}