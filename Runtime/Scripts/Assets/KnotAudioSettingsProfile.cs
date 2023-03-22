using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    [CreateAssetMenu(fileName = "KnotAudioSettingsProfile", menuName = KnotAudio.CoreName + "/Audio Settings Profile", order = -100)]
    public class KnotAudioSettingsProfile : ScriptableObject
    {
        public AudioMixer DefaultAudioMixer => _defaultAudioMixer;
        [SerializeField] protected AudioMixer _defaultAudioMixer;
        
        public bool AudioMixerParameterVolumes => _audioMixerParameterVolumes;
        [SerializeField] protected bool _audioMixerParameterVolumes = true;

        public AudioMixerSnapshot DefaultSnapshot => _defaultSnapshot;
        [SerializeField] protected AudioMixerSnapshot _defaultSnapshot;

        public bool SnapshotVolumes => _snapshotVolumes;
        [SerializeField] protected bool _snapshotVolumes = true;


        public IReadOnlyList<KnotAudioGroup> AudioGroups => _audioGroups ?? (_audioGroups = new List<KnotAudioGroup>());
        [SerializeField, Space] protected List<KnotAudioGroup> _audioGroups;

        public IReadOnlyList<KnotAudioDataLibraryAsset> AudioDataLibraries =>
            _audioDataLibraries ?? (_audioDataLibraries = new List<KnotAudioDataLibraryAsset>());
        [SerializeField] protected List<KnotAudioDataLibraryAsset> _audioDataLibraries;
        

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (KnotAudio.Settings == this)
            {
                RebuildCachedAudioGroupNames();
                RebuildCachedLibraryEntries();
            }
#endif
        }


#if UNITY_EDITOR
        internal static IReadOnlyList<string> CachedAudioGroupNames
        {
            get
            {
                if (_cachedAudioGroupNames == null)
                    RebuildCachedAudioGroupNames();
                return _cachedAudioGroupNames;
            }
        }
        private static List<string> _cachedAudioGroupNames;

        internal static IReadOnlyList<string> CachedLibraryEntryNames
        {
            get
            {
                if (_cachedLibraryEntryNames == null)
                    RebuildCachedLibraryEntries();
                return _cachedLibraryEntryNames;
            }
        }
        private static List<string> _cachedLibraryEntryNames;


        internal static void RebuildCachedAudioGroupNames()
        {
            if (_cachedAudioGroupNames == null)
                _cachedAudioGroupNames = new List<string>();
            else _cachedAudioGroupNames.Clear();

            if (KnotAudio.Settings == null)
                return;
            
            foreach (var audioGroup in KnotAudio.Settings.AudioGroups)
            {
                if (string.IsNullOrEmpty(audioGroup.Name) || _cachedAudioGroupNames.Contains(audioGroup.Name))
                    continue;

                _cachedAudioGroupNames.Add(audioGroup.Name);
            }
        }

        internal static void RebuildCachedLibraryEntries()
        {
            if (_cachedLibraryEntryNames == null)
                _cachedLibraryEntryNames = new List<string>();
            else _cachedLibraryEntryNames.Clear();

            if (KnotAudio.Settings == null)
                return;

            foreach (var library in KnotAudio.Settings.AudioDataLibraries)
            {
                if (library == null)
                    continue;

                foreach (var entry in library)
                {
                    if (entry == null)
                        continue;

                    if (string.IsNullOrEmpty(entry.Name) || _cachedLibraryEntryNames.Contains(entry.Name))
                        continue;

                    _cachedLibraryEntryNames.Add(entry.Name);
                }
            }
        }
#endif
    }
}
