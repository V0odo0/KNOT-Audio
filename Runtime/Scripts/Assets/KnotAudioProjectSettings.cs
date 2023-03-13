#pragma warning disable CS0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    public class KnotAudioProjectSettings : ScriptableObject
    {
        internal static KnotAudioProjectSettings Empty => _empty ?? (_empty = CreateDefault());
        private static KnotAudioProjectSettings _empty;


        public AudioMixer DefaultAudioMixer => _defaultAudioMixer;
        [SerializeField] private AudioMixer _defaultAudioMixer;

        public AudioMixerSnapshot DefaultSnapshot => _defaultSnapshot;
        [SerializeField] private AudioMixerSnapshot _defaultSnapshot;

        public bool EnableVolumes => _enableVolumes;
        [SerializeField] private bool _enableVolumes;

        public IReadOnlyList<KnotAudioGroup> AudioGroups => _audioGroups ?? (_audioGroups = new List<KnotAudioGroup>());
        [SerializeField, Space] private List<KnotAudioGroup> _audioGroups;

        public IReadOnlyList<KnotAudioDataLibraryAsset> AudioDataLibraries =>
            _audioDataLibraries ?? (_audioDataLibraries = new List<KnotAudioDataLibraryAsset>());
        [SerializeField] private List<KnotAudioDataLibraryAsset> _audioDataLibraries;



        public static KnotAudioProjectSettings CreateDefault()
        {
            var instance = CreateInstance<KnotAudioProjectSettings>();

            var uiAudioGroup = new KnotAudioGroup("UI", 
                new KnotDontDestroyOnLoadMod(),
                new KnotSpatialBlendMod(0),
                new KnotBypassConfigMod(true, true, true),
                new KnotAudioListenerConfigMod(true, false));
            instance._audioGroups = new List<KnotAudioGroup>(new[] { uiAudioGroup });

            return instance;
        }


#if UNITY_EDITOR
        internal static string[] CachedAudioGroupNames
        {
            get
            {
                if (_cachedAudioGroupNames == null)
                    RebuildCachedAudioGroupNames();
                return _cachedAudioGroupNames;
            }
        }
        private static string[] _cachedAudioGroupNames;

        internal static List<string> CachedAudioGroupNamesList
        {
            get
            {
                if (_cachedAudioGroupNamesList == null)
                    RebuildCachedAudioGroupNames();
                return _cachedAudioGroupNamesList;
            }
        }
        private static List<string> _cachedAudioGroupNamesList;


        void OnValidate()
        {
            RebuildCachedAudioGroupNames();

            if (KnotAudio.ProjectSettings == this && KnotAudio.Manager != null)
                KnotAudio.Manager.SetProjectSettings(KnotAudio.ProjectSettings);
        }

        static void RebuildCachedAudioGroupNames()
        {
            _cachedAudioGroupNamesList = new List<string>(new []{"None"});
            if (KnotAudio.ProjectSettings == null)
            {
                _cachedAudioGroupNames = _cachedAudioGroupNamesList.ToArray();
                return;
            }

            foreach (var audioGroup in KnotAudio.ProjectSettings.AudioGroups)
            {
                if (string.IsNullOrEmpty(audioGroup.Name))
                    continue;

                _cachedAudioGroupNamesList.Add(audioGroup.Name);
            }

            _cachedAudioGroupNames = _cachedAudioGroupNamesList.ToArray();
        }
#endif
    }
}