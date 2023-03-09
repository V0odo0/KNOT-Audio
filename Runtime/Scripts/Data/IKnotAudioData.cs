using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotAudioData
    {
        AudioClip AudioClip { get; }
        string Group { get; }

        IEnumerable<IKnotAudioDataMod> GetAllMods();
    }
}
