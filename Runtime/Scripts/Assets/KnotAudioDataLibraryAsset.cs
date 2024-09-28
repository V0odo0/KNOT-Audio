using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knot.Core;
using UnityEngine;
using static Knot.Audio.KnotAudioDataLibraryAsset;

namespace Knot.Audio
{
    [KnotTypeInfo("Audio Data Library")]
    [CreateAssetMenu(fileName = "KnotAudioDataLibrary", menuName = KnotAudio.CorePath + "Audio Data Library", order = -100)]
    public class KnotAudioDataLibraryAsset : ScriptableObject, IEnumerable<AudioDataLibraryEntry>
    {
        public List<AudioDataLibraryEntry> Entries => 
            _entries ?? (_entries = new List<AudioDataLibraryEntry>());
        [SerializeField] private List<AudioDataLibraryEntry> _entries;


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<AudioDataLibraryEntry> GetEnumerator() => _entries.GetEnumerator();


        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (KnotAudio.SettingsProfile != null && KnotAudio.SettingsProfile.AudioDataLibraries.Contains(this))
                KnotAudioSettingsProfile.RebuildCachedLibraryEntries();
#endif
        }


        [Serializable]
        public class AudioDataLibraryEntry
        {
            public string Name
            {
                get => _name;
                set => _name = value;
            }
            [SerializeField] private string _name;

            public IKnotPersistentAudioDataProvider Provider
            {
                get => _provider;
                set => _provider = value;
            }
            [SerializeReference, KnotTypePicker(typeof(IKnotPersistentAudioDataProvider))]
            private IKnotPersistentAudioDataProvider _provider;


            public AudioDataLibraryEntry() { }

            public AudioDataLibraryEntry(string name, IKnotPersistentAudioDataProvider provider)
            {
                _name = name;
                _provider = provider;
            }
        }
    }
}
