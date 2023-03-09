using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomPropertyDrawer(typeof(KnotAudioGroupNamePickerAttribute))]
    public class KnotAudioGroupPickerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (KnotAudio.ProjectSettings == null || property.propertyType != SerializedPropertyType.String)
            {
                base.OnGUI(position, property, label);
                return;
            }


            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            int id;
            if (KnotAudioProjectSettings.CachedAudioGroupNames.Contains(property.stringValue))
            {
                id = KnotAudioProjectSettings.CachedAudioGroupNamesList.IndexOf(property.stringValue);
                id = EditorGUI.Popup(position, label.text, id, KnotAudioProjectSettings.CachedAudioGroupNames);
            }
            else id = EditorGUI.Popup(position, label.text, 0, KnotAudioProjectSettings.CachedAudioGroupNames);

            if (EditorGUI.EndChangeCheck())
                property.stringValue = id == 0 ? string.Empty : KnotAudioProjectSettings.CachedAudioGroupNames[id];

            EditorGUI.EndProperty();
        }
    }
}
