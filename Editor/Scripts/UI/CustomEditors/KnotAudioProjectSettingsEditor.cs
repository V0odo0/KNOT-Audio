using Knot.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomEditor(typeof(KnotAudioProjectSettings))]
    internal class KnotAudioProjectSettingsEditor : ProjectSettingsEditor<KnotAudioProjectSettings>
    {
        internal static string SettingsPath = $"Project/KNOT/Audio";
        
        [SettingsProvider]
        static SettingsProvider GetSettingsProvider()
        {
            return GetSettingsProvider(KnotAudio.ProjectSettings, SettingsPath,
                typeof(KnotAudioProjectSettingsEditor));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var customSettings = serializedObject.FindProperty("_customProfile");
            EditorGUILayout.PropertyField(customSettings);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space(10);

            if (customSettings.objectReferenceValue == null)
                base.OnInspectorGUI();
        }


        public static void Open()
        {
            SettingsService.OpenProjectSettings(SettingsPath);
        }
    }
}