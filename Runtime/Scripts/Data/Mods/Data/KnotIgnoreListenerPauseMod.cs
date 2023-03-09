using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Ignore Listener Pause")]
    public class KnotIgnoreListenerPauseMod : IKnotAudioDataMod
    {
        public void Initialize(KnotAudioSource source)
        {
            source.AudioSource.ignoreListenerPause = true;
        }
    }
}
