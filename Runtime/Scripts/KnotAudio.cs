using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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


        public static IReadOnlyDictionary<string, KnotAudioGroup> AudioGroups =>
            _audioGroups ?? (_audioGroups = new Dictionary<string, KnotAudioGroup>());
        private static Dictionary<string, KnotAudioGroup> _audioGroups;

        internal static KnotAudioManager Manager => _manager;
        private static KnotAudioManager _manager;
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            if (ProjectSettings == null)
                return;

            _manager = GetAudioManager();
            _audioGroups = new Dictionary<string, KnotAudioGroup>();
            foreach (var audioGroup in ProjectSettings.AudioGroups)
            {
                if (string.IsNullOrEmpty(audioGroup.Name))
                    continue;

                if (_audioGroups.ContainsKey(audioGroup.Name))
                    _audioGroups[audioGroup.Name] = audioGroup;
                else _audioGroups.Add(audioGroup.Name, audioGroup);
            }
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
            manager.Initialize(ProjectSettings);
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



        public static KnotAudioSource PlayOnce(IKnotAudioDataProvider provider, params IKnotAudioMod[] mods)
        {
            if (provider == null)
                return null;

            return PlayOnce(provider.AudioData, mods);
        }

        public static KnotAudioSource PlayOnce(IKnotAudioData data, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.PlayOnce(data, mods);

        public static KnotAudioSource PlayOnce(AudioClip clip, params IKnotAudioMod[] mods) =>
            Manager == null ? null : Manager.PlayOnce(clip, mods);

        
        internal class KnotAudioManager : MonoBehaviour
        {
            public Dictionary<string, KnotAudioGroup> AudioGroups => _audioGroups ?? (_audioGroups = new Dictionary<string, KnotAudioGroup>());
            private Dictionary<string, KnotAudioGroup> _audioGroups;


            void OnDestroy()
            {
                _manager = null;
            }

            KnotAudioSource InstantiateAudioSource()
            {
                var audioSource = new GameObject(nameof(KnotAudioSource)).AddComponent<KnotAudioSource>();
                audioSource.transform.SetParent(transform);

                return audioSource;
            }

            public void Initialize(KnotAudioProjectSettings settings)
            {
                foreach (var audioGroup in settings.AudioGroups)
                {
                    if (audioGroup == null || string.IsNullOrEmpty(audioGroup.Name))
                        continue;
                }
            }

            public KnotAudioSource PlayOnce(IKnotAudioData data, params IKnotAudioMod[] mods)
            {
                if (data == null || data.AudioClip == null)
                    return null;

                return InstantiateAudioSource().Initialize(data, mods).PlayOnce();
            }

            public KnotAudioSource PlayOnce(AudioClip clip, params IKnotAudioMod[] mods)
            {
                if (clip == null)
                    return null;

                return InstantiateAudioSource().Initialize(new KnotAudioData(clip), mods).PlayOnce();
            }
        }
    }
}

