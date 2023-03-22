using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;
using static Knot.Audio.KnotAudioMixerParametersVolume;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Knot.Audio
{
    public static class KnotAudio
    {
        internal const string CoreName = "KNOT Audio";
        internal static Color DefaultGizmosColor { get; } = new Color(1, 1, 0, 0.33f);

        internal static KnotAudioProjectSettings ProjectSettings =>
            _projectSettings ?? (_projectSettings = LoadProjectSettings());
        private static KnotAudioProjectSettings _projectSettings;
        
        public static KnotAudioSettingsProfile Settings
        {
            get
            {
                if (Manager == null || Manager.OverrideSettings == null)
                    return ProjectSettings.CustomSettings == null ? _projectSettings : _projectSettings.CustomSettings;
                return Manager.OverrideSettings;
            }
            set
            {
                if (Manager == null || Manager.OverrideSettings == value)
                    return;

                var prev = Manager.OverrideSettings;
                Manager.OverrideSettings = value;
                Manager.SetSettings(value == null && prev != null ? Settings : Manager.OverrideSettings);
            }
        }

        public static Transform AudioListener
        {
            get
            {
                if (Manager == null)
                    return null;

                if (Manager.OverrideAudioListener != null)
                    return Manager.OverrideAudioListener;

                if (_audioListener == null)
                {
                    var listener = GameObject.FindObjectOfType<AudioListener>();
                    if (listener != null)
                        _audioListener = listener.transform;
                }
                return _audioListener;
            }
            set
            {
                if (Manager == null)
                    return;

                Manager.OverrideAudioListener = value;
            }
        }
        private static Transform _audioListener;

        internal static KnotAudioManager Manager => _manager;
        private static KnotAudioManager _manager;
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            if (Settings == null)
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
            else
            {
                settings = allSettings.FirstOrDefault(p => p.name.Equals(nameof(KnotAudioProjectSettings)));
                if (settings == null)
                    settings = allSettings.First();
            }
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
            manager.SetSettings(Settings);
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


        public static KnotAudioControllerHandle Play(this KnotAudioDataReference reference, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods) =>
            reference == null ? default : Play(reference.Provider, playMode, mods);

        public static KnotAudioControllerHandle Play(this IKnotAudioDataProvider provider, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods) =>
            provider == null ? default : Play(provider.AudioData, playMode,mods);

        public static KnotAudioControllerHandle Play(this IKnotAudioData data, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods) =>
            Manager == null ? default : Manager.Play(data, playMode, mods);

        public static KnotAudioControllerHandle Play(this AudioClip clip, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot,  params IKnotAudioMod[] mods) =>
            Manager == null ? default : Manager.Play(clip, playMode, mods);

        public static KnotAudioControllerHandle Play(this AudioClip clip, string audioGroupName, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods) =>
            Manager == null ? default : Manager.Play(clip, audioGroupName, playMode, mods);

        public static KnotAudioControllerHandle Play(string libraryEntryName, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods) =>
            Manager == null ? default : Manager.Play(libraryEntryName, playMode, mods);


        public static IEnumerable<IKnotAudioGroupMod> GetAudioGroupMods(string groupName) =>
            Manager == null ? Array.Empty<IKnotAudioGroupMod>() : Manager.GetAudioGroupMods(groupName);

        public static KnotAudioGroup GetAudioGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;

            if (Manager == null)
                return Settings.AudioGroups.FirstOrDefault(g => g.Name == groupName);

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
                return Settings.AudioDataLibraries.SelectMany(p => p.Entries).FirstOrDefault(g => g.Name == name)?.Provider?.AudioData;

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
            public KnotAudioSettingsProfile OverrideSettings { get; set; }
            public Transform OverrideAudioListener { get; set; }

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
            
            private Dictionary<AudioMixerSnapshot, float> _snapshotWeights = new Dictionary<AudioMixerSnapshot, float>();
            private List<AudioMixerSnapshot> _activeSnapshotVolumes = new List<AudioMixerSnapshot>();

            private Dictionary<string, float> _mixerDefaultParams = new Dictionary<string, float>();
            private Dictionary<string, float> _mixerBlendParams = new Dictionary<string, float>();
            private List<string> _curActiveMixerParameters = new List<string>();
            private List<string> _lastActiveMixerParameters = new List<string>();



            private Dictionary<KnotInstanceLimitMod, HashSet<KnotAudioControllerBase>> _limitInstances =
                new Dictionary<KnotInstanceLimitMod, HashSet<KnotAudioControllerBase>>();

            void Update()
            {
                UpdateSnapshotVolumes();
                UpdateMixerParameterVolumes();
            }

            void OnDestroy()
            {
                _manager = null;
            }


            void UpdateSnapshotVolumes()
            {
                if (!Settings.SnapshotVolumes || Settings.DefaultSnapshot == null || AudioListener == null)
                    return;

                _activeSnapshotVolumes.Clear();
                foreach (var volume in KnotAudioMixerSnapshotVolume.ActiveInstances.Where(v => v.Snapshot != null))
                {
                    var weight = volume.GetWeight(AudioListener.position);
                    if (_activeSnapshotVolumes.Contains(volume.Snapshot))
                    {
                        if (_snapshotWeights[volume.Snapshot] > weight)
                            weight = _snapshotWeights[volume.Snapshot];
                    }
                    else _activeSnapshotVolumes.Add(volume.Snapshot);

                    if (!_snapshotWeights.ContainsKey(volume.Snapshot))
                        _snapshotWeights.Add(volume.Snapshot, weight);
                    else _snapshotWeights[volume.Snapshot] = weight;
                }

                foreach (var snapshot in _snapshotWeights.Keys.ToArray())
                    if (!_activeSnapshotVolumes.Contains(snapshot))
                        _snapshotWeights[snapshot] = 0;

                if (!_snapshotWeights.ContainsKey(Settings.DefaultSnapshot))
                    _snapshotWeights.Add(Settings.DefaultSnapshot, 1);
                _snapshotWeights[Settings.DefaultSnapshot] = 1 - _snapshotWeights.Values.Max();

                Settings.DefaultSnapshot.audioMixer.TransitionToSnapshots(_snapshotWeights.Keys.ToArray(), _snapshotWeights.Values.ToArray(), 0);
            }

            void UpdateMixerParameterVolumes()
            {
                if (!Settings.AudioMixerParameterVolumes || Settings.DefaultAudioMixer == null || AudioListener == null)
                    return;

                _curActiveMixerParameters.Clear();
                
                _mixerBlendParams.Clear();
                foreach (var p in _mixerDefaultParams)
                    _mixerBlendParams.Add(p.Key, p.Value);
                
                foreach (var vol in KnotAudioMixerParametersVolume.ActiveInstances.OrderBy(v => v.Priority))
                {
                    float weight = vol.GetWeight(AudioListener.transform.position);
                    foreach (var parameter in vol.Parameters)
                    {
                        if (Settings.DefaultAudioMixer.GetFloat(parameter.Name, out var currentValue))
                        {
                            if (!_mixerDefaultParams.ContainsKey(parameter.Name))
                            {
                                _mixerDefaultParams.Add(parameter.Name, currentValue);
                                _mixerBlendParams.Add(parameter.Name, currentValue);
                            }

                            var blendValue = Mathf.Lerp(_mixerBlendParams[parameter.Name], 
                                parameter.TargetValue, weight);
                            _mixerBlendParams[parameter.Name] = blendValue;

                            Settings.DefaultAudioMixer.SetFloat(parameter.Name, blendValue);
                            _curActiveMixerParameters.Add(parameter.Name);
                        }
                    }
                }

                if (_curActiveMixerParameters.Count < _lastActiveMixerParameters.Count)
                {
                    foreach (var p in _lastActiveMixerParameters.Where(s => !_curActiveMixerParameters.Contains(s)))
                        if (_mixerBlendParams.TryGetValue(p, out var val))
                            Settings.DefaultAudioMixer.SetFloat(p, val);
                }

                _lastActiveMixerParameters.Clear();
                _lastActiveMixerParameters.AddRange(_curActiveMixerParameters);
            }

            public IEnumerable<IKnotAudioGroupMod> GetAudioGroupMods(string groupName)
            {
                return GetAudioGroup(groupName)?.Mods?.OfType<IKnotAudioGroupMod>() ?? Array.Empty<IKnotAudioGroupMod>();
            }

            KnotAudioControllerBase GetControllerInstance(IEnumerable<IKnotAudioMod> allMods)
            {
                KnotAudioControllerBase controller = null;
                var playChance = allMods.OfType<KnotPlayChanceMod>().LastOrDefault();
                if (playChance != null && !playChance.SampleCanPlay())
                    return null;
                
                var instanceLimit = allMods.OfType<KnotInstanceLimitMod>().LastOrDefault();
                if (instanceLimit != null)
                {
                    if (_limitInstances.TryGetValue(instanceLimit, out var instances))
                    {
                        instances.RemoveWhere(c => c == null);
                        if (instances.Count >= instanceLimit.InstanceLimit)
                        {
                            switch (instanceLimit.LimitSolveMethod)
                            {
                                case KnotInstanceLimitMod.InstanceLimitSolveMethod.DestroyOld:
                                    var oldestInstance = instances.OrderBy(c => c.InstanceCreationTimestamp)
                                        .FirstOrDefault();
                                    if (oldestInstance != null)
                                        Destroy(oldestInstance.gameObject);
                                    break;
                                case KnotInstanceLimitMod.InstanceLimitSolveMethod.DestroyMostDistant:
                                    if (AudioListener == null)
                                        goto case KnotInstanceLimitMod.InstanceLimitSolveMethod.DonNotPlayNew;

                                    KnotAudioControllerBase distantInstance = null;
                                    float maxDistance = 0;
                                    foreach (var instance in instances)
                                    {
                                        var curDistance = Vector3.Distance(AudioListener.position, instance.transform.position);
                                        if (curDistance > maxDistance)
                                        {
                                            maxDistance = curDistance;
                                            distantInstance = instance;
                                        }
                                    }

                                    if (distantInstance != null)
                                        Destroy(distantInstance.gameObject);
                                    break;
                                case KnotInstanceLimitMod.InstanceLimitSolveMethod.DonNotPlayNew:
                                    return null;
                            }
                        }
                    }
                }

                var template = allMods.OfType<KnotAudioSourceTemplateMod>().LastOrDefault();
                if (template == null || template.Template == null)
                    controller = new GameObject(nameof(KnotAudioController)).AddComponent<KnotAudioController>();
                else
                {
                    var audioSource = Instantiate(template.Template);
                    controller = audioSource.gameObject.AddComponent<KnotAudioController>();
                }

                if (instanceLimit != null)
                {
                    if (!_limitInstances.ContainsKey(instanceLimit))
                        _limitInstances.Add(instanceLimit, new HashSet<KnotAudioControllerBase>());
                    _limitInstances[instanceLimit].Add(controller);
                }

                return controller;
            }


            public void SetSettings(KnotAudioSettingsProfile settings)
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

            
            public KnotAudioControllerHandle Play(string libraryEntryName, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods)
            {
                var entry = GetLibraryEntry(libraryEntryName);
                return entry == null ? 
                    default : 
                    Play(entry, playMode, mods);
            }

            public KnotAudioControllerHandle Play(IKnotAudioData data, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods)
            {
                if (data == null || data.AudioClip == null)
                    return default;

                var controller = GetControllerInstance(GetAudioGroupMods(data.GroupName).Union(data.GetAllMods().Union(mods)));
                return controller == null ? 
                    default : 
                    new KnotAudioControllerHandle(controller.InitAsInstance(data, playMode).AppendMods(mods));
            }

            public KnotAudioControllerHandle Play(AudioClip clip, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods)
            {
                var controller = GetControllerInstance(mods);
                return controller == null ? 
                    default : 
                    new KnotAudioControllerHandle(controller.InitAsInstance(new KnotAudioData(clip), playMode).AppendMods(mods));
            }

            public KnotAudioControllerHandle Play(AudioClip clip, string audioGroupName, KnotAudioPlayMode playMode = KnotAudioPlayMode.OneShot, params IKnotAudioMod[] mods)
            {
                var allMods = GetAudioGroupMods(audioGroupName).Union(mods);
                var controller = GetControllerInstance(allMods);
                return controller == null ? 
                    default : 
                    new KnotAudioControllerHandle(controller.InitAsInstance(new KnotAudioData(clip, audioGroupName), playMode).AppendMods(mods));
            }
        }
    }
}

