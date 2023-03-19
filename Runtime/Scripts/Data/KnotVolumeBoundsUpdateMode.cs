using System;

namespace Knot.Audio
{
    [Serializable]
    public enum KnotVolumeBoundsUpdateMode 
    {
        OnceOnAwake,
        EveryFrame,
        Manual
    }
}
