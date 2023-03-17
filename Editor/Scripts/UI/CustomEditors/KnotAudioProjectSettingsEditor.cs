using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomEditor(typeof(KnotAudioProjectSettings))]
    public class KnotAudioProjectSettingsEditor : UnityEditor.Editor
    {
        internal static string SettingsPath = $"Project/KNOT/Audio";
        internal static string[] DefaultKeyWords = 
        {
            "knot",
            "audio",
            "sound"
        };


        public override void OnInspectorGUI()
        {
            if (target == null)
                return;

            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    if (property.name == "m_Script")
                        continue;

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(property.name), true);
                }
                while (property.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }


        [SettingsProvider]
        static SettingsProvider GetSettingsProvider()
        {
            var provider = new SettingsProvider(SettingsPath, SettingsScope.Project, DefaultKeyWords);
            var editor = CreateEditor(KnotAudio.ProjectSettings);
            SerializedObject sObj = new SerializedObject(KnotAudio.ProjectSettings);
            
            provider.guiHandler += s =>
            {
                var customProfileProperty = sObj.FindProperty("_customSettings");

                sObj.Update();
                EditorGUILayout.PropertyField(customProfileProperty);
                sObj.ApplyModifiedProperties();

                EditorGUILayout.Space();
                if (customProfileProperty.objectReferenceValue == null)
                {
                    EditorGUILayout.LabelField("Default Settings", EditorStyles.boldLabel);
                    editor.OnInspectorGUI();
                }
            };

            return provider;
        }


        public static void Open()
        {
            SettingsService.OpenProjectSettings(SettingsPath);
        }
    }
}