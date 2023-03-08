using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotAudioData
    {
        AudioClip AudioClip { get; }
        

        IEnumerable<IKnotAudioDataMod> GetAllMods();
    }
}
