using System.Collections;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("Mod Collection Asset")]
    [CreateAssetMenu(fileName = "KnotModCollectionAsset", menuName = KnotAudio.CoreName + "/Mod Collection", order = -1000)]
    public class KnotModCollectionAsset : ScriptableObject, IEnumerable<IKnotAudioDataMod>
    {
        public IList<IKnotAudioDataMod> Mods => _mods ?? (_mods = new List<IKnotAudioDataMod>());
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataMod))] private List<IKnotAudioDataMod> _mods;


        public IEnumerator GetEnumerator() => Mods.GetEnumerator();

        IEnumerator<IKnotAudioDataMod> IEnumerable<IKnotAudioDataMod>.GetEnumerator() => Mods.GetEnumerator();
    }
}
