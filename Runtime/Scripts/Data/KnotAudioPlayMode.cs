using System;

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