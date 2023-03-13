using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Knot.Audio
{
    public static class KnotAudio
    {
        internal const string CoreName = "KNOT Audio";

        public static KnotAudioProjectSettings ProjectSettings =>
            _projectSettings ?? (_projectSettings = LoadProjectSettings());
        private static KnotAudioProjectSettings _projectSettings;

        internal static KnotAudioManager Manager => _manager;
        private static KnotAudioManager _manager;
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            if (ProjectSettings == null)
                return;

            _manager = GetAudioManager();
        }

        static KnotAudioProjectSettings LoadProjectSettings()
        {
            KnotAudioProjectSettings settings;

#if UNITY_EDITOR
            var allSettings =
                AssetDatabase.FindAssets($"t:{nameof(KnotAudioProjectSettings)}").
                    Select(AssetDatabase.GUIDToAssetPath).
                    Select(AssetDatabase.LoadAssetAtPath<KnotAudioProjectSettings>).ToArray();

            if (allSettings.Length == 0)
            {
                string path = $"Assets/{nameof(KnotAudioProjectSettings)}.asset";
                settings = AssetDatabase.LoadAssetAtPath<KnotAudioProjectSettings>(path);

                if (settings == null)
                    settings = PlayerSettings.GetPreloadedAssets().OfType<KnotAudioProjectSettings>().FirstOrDefault();

                if (settings == null)
                {
                    var instance = KnotAudioProjectSettings.CreateDefault();
                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.SaveAssets();
                    settings = instance;

                    var preloadedAssets = PlayerSettings.GetPreloadedAssets();
                    PlayerSettings.SetPreloadedAssets(preloadedAssets.Append(settings).ToArray());
                }
            }
            else settings = allSettings.First();
#else
            settings = Resources.FindObjectsOfTypeAll<KnotAudioProjectSettings>().FirstOrDefault();
#endif

            if (settings == null)
            {
                settings = KnotAudioProjectSettings.Empty;
                Log("Unable to load or create Project Settings. Empty Project Settings will be assigned.", LogType.Warning);
            }
            return settings;
        }

        static KnotAudioManager GetAudioManager()
        {
            var manager = new GameObject(nameof(KnotAudioManager)).AddComponent<KnotAudioManager>();
            manager.SetProjectSettings(ProjectSettings);
            Object.DontDestroyOnLoad(manager);

            return manager;
        }

        internal static void Log(string message, LogType type)
        {
            message = $"{CoreName}: {message}";
            switch (type)
            {
                default:
                    Debug.Log(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
            }
        }


        public static KnotNativeAudioSourceController PlayOnce(this KnotAudioDataReference reference, params IKnotAudioMod[] mods) =>
            reference == null ? null : PlayOnce(reference.Provider, mods);

        public static KnotNativeAudioSourceController PlayOnce(this IKnotAudioDataProvider provider, params IKnotAudioMod[] mods) =>
            provider == null ? null : PlayOnce(provider.AudioData, mods);

        public static KnotNativeAudioSourceController PlayOnce(this IKnotAudioData data, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(data, false, mods);

        public static KnotNativeAudioSourceController PlayOnce(this AudioClip clip, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(clip, false, mods);

        public static KnotNativeAudioSourceController PlayOnce(string libraryEntryName, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(libraryEntryName, false, mods);

        public static KnotNativeAudioSourceController PlayLoop(this KnotAudioDataReference reference, params IKnotAudioMod[] mods) =>
            reference == null ? null : PlayLoop(reference.Provider, mods);
        
        public static KnotNativeAudioSourceController PlayLoop(this IKnotAudioDataProvider provider, params IKnotAudioMod[] mods) =>
            provider == null ? null : Manager.Play(provider.AudioData, true, mods);
        
        public static KnotNativeAudioSourceController PlayLoop(this IKnotAudioData data, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(data, true, mods);

        public static KnotNativeAudioSourceController PlayLoop(this AudioClip clip, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(clip, true, mods);

        public static KnotNativeAudioSourceController PlayLoop(string libraryEntryName, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.Play(libraryEntryName, true, mods);


        public static KnotAudioGroup GetAudioGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;

            if (Manager == null)
                return ProjectSettings.AudioGroups.FirstOrDefault(g => g.Name == groupName);

            if (Manager.AudioGroupsOverrides.TryGetValue(groupName, out var groupOverride))
                return groupOverride;

            return Manager.AudioGroups.TryGetValue(groupName, out var group) ? group : null;
        }

        public static bool AddOverrideAudioGroup(KnotAudioGroup group)
        {
            if (Manager == null || group == null || string.IsNullOrEmpty(group.Name))
                return false;

            if (Manager.AudioGroupsOverrides.ContainsKey(group.Name))
                Manager.AudioGroupsOverrides[group.Name] = group;
            else Manager.AudioGroupsOverrides.Add(group.Name, group);

            return true;
        }

        public static bool RemoveOverrideAudioGroup(string groupName)
        {
            if (Manager == null || string.IsNullOrEmpty(groupName))
                return false;

            if (!Manager.AudioGroupsOverrides.ContainsKey(groupName))
                return false;

            Manager.AudioGroupsOverrides.Remove(groupName);

            return true;
        }

        public static IKnotAudioData GetLibraryEntry(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (Manager == null)
                return ProjectSettings.AudioDataLibraries.SelectMany(p => p.Entries).FirstOrDefault(g => g.Name == name)?.Provider?.AudioData;

            if (Manager.LibraryEntriesOverrides.TryGetValue(name, out var dataOverride))
                return dataOverride;

            return Manager.LibraryEntries.TryGetValue(name, out var data) ? data : null;
        }

        public static bool AddOverrideAudioLibraryEntry(string name, KnotAudioData data)
        {
            if (Manager == null || data == null || string.IsNullOrEmpty(name))
                return false;

            if (Manager.LibraryEntriesOverrides.ContainsKey(name))
                Manager.LibraryEntriesOverrides[name] = data;
            else Manager.LibraryEntriesOverrides.Add(name, data);

            return true;
        }

        public static bool RemoveOverrideAudioLibraryEntry(string name)
        {
            if (Manager == null || string.IsNullOrEmpty(name))
                return false;
            
            if (!Manager.LibraryEntriesOverrides.ContainsKey(name))
                return false;

            Manager.LibraryEntriesOverrides.Remove(name);

            return true;
        }


        internal class KnotAudioManager : MonoBehaviour
        {
            public Dictionary<string, IKnotAudioData> LibraryEntries => _libraryEntries ??
                (_libraryEntries = new Dictionary<string, IKnotAudioData>());
            private Dictionary<string, IKnotAudioData> _libraryEntries;

            public Dictionary<string, IKnotAudioData> LibraryEntriesOverrides => _libraryEntriesOverrides ??
                (_libraryEntriesOverrides = new Dictionary<string, IKnotAudioData>());
            private Dictionary<string, IKnotAudioData> _libraryEntriesOverrides;

            public Dictionary<string, KnotAudioGroup> AudioGroups => _audioGroups ?? (_audioGroups = new Dictionary<string, KnotAudioGroup>());
            private Dictionary<string, KnotAudioGroup> _audioGroups;

            public Dictionary<string, KnotAudioGroup> AudioGroupsOverrides => _audioGroupsOverrides ?? (_audioGroupsOverrides = new Dictionary<string, KnotAudioGroup>());
            private Dictionary<string, KnotAudioGroup> _audioGroupsOverrides;

            public AudioListener AudioListener => _audioListener ?? (_audioListener = FindObjectOfType<AudioListener>());
            private AudioListener _audioListener;

            private Dictionary<AudioMixerSnapshot, float> _snapshotWeights = new Dictionary<AudioMixerSnapshot, float>();
            private List<AudioMixerSnapshot> _activeSnapshotVolumes = new List<AudioMixerSnapshot>();


            void Update()
            {
                UpdateAudioMixerSnapshotVolumes();
            }

            void OnDestroy()
            {
                _manager = null;
            }


            void UpdateAudioMixerSnapshotVolumes()
            {
                if (!ProjectSettings.EnableVolumes || ProjectSettings.DefaultSnapshot == null || AudioListener == null)
                    return;

                _activeSnapshotVolumes.Clear();
                foreach (var instance in KnotAudioMixerSnapshotVolume.ActiveInstances)
                {
                    if (instance.Snapshot == null)
                        continue;

                    var weight = instance.GetWeight(this.AudioListener.transform.position);
                    if (_activeSnapshotVolumes.Contains(instance.Snapshot))
                    {
                        if (_snapshotWeights[instance.Snapshot] > weight)
                            weight = _snapshotWeights[instance.Snapshot];
                    }
                    else _activeSnapshotVolumes.Add(instance.Snapshot);

                    if (!_snapshotWeights.ContainsKey(instance.Snapshot))
                        _snapshotWeights.Add(instance.Snapshot, weight);
                    else _snapshotWeights[instance.Snapshot] = weight;
                }

                foreach (var snapshot in _snapshotWeights.Keys.ToArray())
                    if (!_activeSnapshotVolumes.Contains(snapshot))
                        _snapshotWeights[snapshot] = 0;

                if (!_snapshotWeights.ContainsKey(ProjectSettings.DefaultSnapshot))
                    _snapshotWeights.Add(ProjectSettings.DefaultSnapshot, 1);
                _snapshotWeights[ProjectSettings.DefaultSnapshot] = 1 - _snapshotWeights.Values.Max();

                ProjectSettings.DefaultSnapshot.audioMixer.TransitionToSnapshots(_snapshotWeights.Keys.ToArray(), _snapshotWeights.Values.ToArray(), 0);
            }

            KnotNativeAudioSourceController InstantiateAudioSource()
            {
                var audioSource = new GameObject(nameof(KnotNativeAudioSourceController)).AddComponent<KnotNativeAudioSourceController>();
                return audioSource;
            }


            public void SetProjectSettings(KnotAudioProjectSettings settings)
            {
                AudioGroups.Clear();
                foreach (var audioGroup in settings.AudioGroups)
                {
                    if (string.IsNullOrEmpty(audioGroup.Name))
                        continue;

                    if (AudioGroups.ContainsKey(audioGroup.Name))
                        AudioGroups[audioGroup.Name] = audioGroup;
                    else AudioGroups.Add(audioGroup.Name, audioGroup);
                }

                LibraryEntries.Clear();
                foreach (var entry in settings.AudioDataLibraries.Where(a => a != null).SelectMany(e => e.Entries).Where(e => e != null))
                {
                    if (string.IsNullOrEmpty(entry.Name) || entry.Provider == null || entry.Provider.AudioData == null)
                        continue;
                    
                    if (LibraryEntries.ContainsKey(entry.Name))
                        LibraryEntries[entry.Name] = entry.Provider.AudioData;
                    else LibraryEntries.Add(entry.Name, entry.Provider.AudioData);
                }
            }

            
            public KnotNativeAudioSourceController Play(string libraryEntryName, bool loop, params IKnotAudioMod[] mods)
            {
                if (string.IsNullOrEmpty(libraryEntryName))
                    return null;

                if (LibraryEntriesOverrides.TryGetValue(libraryEntryName, out var overrideEntry) && overrideEntry != null)
                    return Play(overrideEntry, loop, mods);

                if (LibraryEntries.TryGetValue(libraryEntryName, out var entry) && entry != null)
                    return Play(entry, loop, mods);

                return null;
            }

            public KnotNativeAudioSourceController Play(IKnotAudioData data, bool loop, params IKnotAudioMod[] mods)
            {
                if (data == null || data.AudioClip == null)
                    return null;

                return InstantiateAudioSource().Initialize(data, mods).Play(loop);
            }

            public KnotNativeAudioSourceController Play(AudioClip clip, bool loop, params IKnotAudioMod[] mods)
            {
                if (clip == null)
                    return null;

                return InstantiateAudioSource().Initialize(new KnotAudioData(clip), mods).Play(loop);
            }
        }
    }
}

