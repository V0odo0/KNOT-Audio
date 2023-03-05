using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


namespace Knot.Audio.Editor
{
    public static class KnotEditorUtils
    {
        public static bool IsUpmPackage => UpmPackageInfo != null;

        public static PackageInfo UpmPackageInfo => 
            _upmPackageInfo ?? (_upmPackageInfo = PackageInfo.FindForAssembly(Assembly.GetAssembly(typeof(KnotAudio))));
        private static PackageInfo _upmPackageInfo;

        /*public static IReadOnlyDictionary<KnotMetadataInfoAttribute.MetadataScope, KnotEditorExtensions.TypeInfo[]> MetadataTypes
        {
            get
            {
                if (_metadataTypes == null)
                {
                    _metadataTypes = new Dictionary<KnotMetadataInfoAttribute.MetadataScope, KnotEditorExtensions.TypeInfo[]>();

                    var allTypesInfo = typeof(IKnotMetadata).GetDerivedTypesInfo();
                    var allScopes = (KnotMetadataInfoAttribute.MetadataScope[]) Enum.GetValues(typeof(KnotMetadataInfoAttribute.MetadataScope));
                    foreach (var scope in allScopes)
                    {
                        List<KnotEditorExtensions.TypeInfo> scopeTypesInfo = new List<KnotEditorExtensions.TypeInfo>();

                        foreach (var typeInfo in allTypesInfo)
                        {
                            var metadataAttribute = typeInfo.Type.GetCustomAttribute<KnotMetadataInfoAttribute>();
                            if (metadataAttribute != null && metadataAttribute.Scope.HasFlag(scope))
                                scopeTypesInfo.Add(typeInfo);
                        }

                        _metadataTypes.Add(scope, scopeTypesInfo.ToArray());
                    }
                }
                return _metadataTypes;
            }
        }
        private static Dictionary<KnotMetadataInfoAttribute.MetadataScope, KnotEditorExtensions.TypeInfo[]> _metadataTypes;
        

        public static KnotEditorUserSettings UserSettings => 
            _userSettings ?? (_userSettings = KnotEditorUserSettings.Load());
        private static KnotEditorUserSettings _userSettings;


        public static Dictionary<string, VisualTreeAsset> EditorPanels
        {
            get
            {
                if (_editorPanels == null || !_editorPanels.Any())
                {
                    _editorPanels = new Dictionary<string, VisualTreeAsset>();
                    foreach (var visualTree in Resources.LoadAll<VisualTreeAsset>("UI").GroupBy(a => a.name).Select(g => g.First()))
                        _editorPanels.Add(visualTree.name, visualTree);
                }
                return _editorPanels;
            }
        }
        private static Dictionary<string, VisualTreeAsset> _editorPanels;

        public static StyleSheet EditorStyles => 
            _editorStyles == null ? _editorStyles = Resources.Load(EditorStylesResourcesPath) as StyleSheet : _editorStyles;
        private static StyleSheet _editorStyles;*/

        internal static MethodInfo GetIconActiveStateMethod => _getIconActiveStateMethod ?? (_getIconActiveStateMethod =
            typeof(EditorUtility).GetMethod(
                "GetIconInActiveState",
                BindingFlags.Static | BindingFlags.NonPublic));
        private static MethodInfo _getIconActiveStateMethod;

        internal static Type AudioUtilsType
        {
            get
            {
                if (_audioUtilsType == null)
                {
                    Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
                    _audioUtilsType = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
                }

                return _audioUtilsType;
            }
        }
        private static Type _audioUtilsType;

        internal static Texture CoreIcon => GetIcon("KnotLocalization_icon");

        private static Dictionary<string, Texture> _cachedIcons = new Dictionary<string, Texture>();
        private static Dictionary<string, Texture> _cachedIconsActiveState = new Dictionary<string, Texture>();


        [InitializeOnLoadMethod]
        static void Init()
        {
            AssemblyReloadEvents.beforeAssemblyReload += SaveAllSettings;
            EditorApplication.quitting += SaveAllSettings;
        }

        static void SaveAllSettings()
        {
            //_userSettings?.Save();
        }


        public static Object RequestCreateAsset(Type type, string name = "", bool ping = false, bool select = false)
        {
            if (type == null || !type.IsSubclassOf(typeof(ScriptableObject)))
                return null;

            name = string.IsNullOrEmpty(name) ? type.Name : name;
            try
            {
                string path = EditorUtility.SaveFilePanelInProject($"Create {name}", name, "asset", "");
                if (string.IsNullOrEmpty(path))
                    return null;

                var instance = ScriptableObject.CreateInstance(type);
                instance.name = name;
                AssetDatabase.CreateAsset(instance, path);

                if (ping)
                    EditorGUIUtility.PingObject(instance);
                if (select)
                    Selection.activeObject = instance;

                return instance;
            }
            catch
            {
                KnotAudio.Log($"Failed to create {name} asset", LogType.Error);
            }

            return null;
        }

        public static T RequestCreateAsset<T>(string name = "", bool ping = false, bool select = false) where T : ScriptableObject
        {
            return RequestCreateAsset(typeof(T), name, ping, select) as T;
        }

        public static bool RecordObjects(string commandName, Action postRecordAction = null, params Object[] obj)
        {
            if (obj == null || obj.Length == 0)
                return false;
            
            obj = obj.Where(o => o != null).ToArray();

            Undo.RecordObjects(obj, $"{KnotAudio.CoreName}: {commandName}");
            postRecordAction?.Invoke();
            foreach (var o in obj)
                EditorUtility.SetDirty(o);

            return true;
        }

        public static bool RegisterCompleteObjects(string commandName, Action postRegisterAction = null, params Object[] obj)
        {
            if (obj == null || obj.Length == 0)
                return false;

            obj = obj.Where(o => o != null).ToArray();

            Undo.RegisterCompleteObjectUndo(obj, $"{KnotAudio.CoreName}: {commandName}");
            postRegisterAction?.Invoke();
            foreach (var o in obj)
                EditorUtility.SetDirty(o);

            return true;
        }

        public static Texture GetIcon(string iconName)
        {
            if (_cachedIcons.ContainsKey(iconName))
                return _cachedIcons[iconName];
            
            Debug.unityLogger.logEnabled = false;
            Texture icon = EditorGUIUtility.IconContent(iconName)?.image;
            Debug.unityLogger.logEnabled = true;

            if (icon == null)
                icon = Resources.Load<Texture>(iconName);

            if (icon == null)
                return null;

            if (!_cachedIcons.ContainsKey(iconName))
                _cachedIcons.Add(iconName, icon);

            return icon;
        }
        
        public static Texture GetIconActiveState(string iconName)
        {
            if (_cachedIconsActiveState.ContainsKey(iconName))
                return _cachedIconsActiveState[iconName];

            if (GetIconActiveStateMethod == null)
                return GetIcon(iconName);

            Debug.unityLogger.logEnabled = false;
            Texture2D icon = (Texture2D) GetIconActiveStateMethod.Invoke(null, new object[]{ GetIcon(iconName) });
            Debug.unityLogger.logEnabled = true;

            if (icon == null)
                return GetIcon(iconName);

            if (!_cachedIconsActiveState.ContainsKey(iconName))
                _cachedIconsActiveState.Add(iconName, icon);

            return icon;
        }

        public static void PlayPreviewClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            MethodInfo method = AudioUtilsType.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] {
                    typeof(AudioClip),
                    typeof(Int32),
                    typeof(Boolean)
                },
                null
            );

            method?.Invoke(null, new object[] { clip, startSample, loop });
        }

        public static void StopAllPreviewClips()
        {
            MethodInfo method = AudioUtilsType.GetMethod(
                "StopAllPreviewClips",
                BindingFlags.Static | BindingFlags.Public
            );

            method?.Invoke(null, null);
        }
    }
}