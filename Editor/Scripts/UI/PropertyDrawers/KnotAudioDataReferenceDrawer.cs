using Knot.Audio.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomPropertyDrawer(typeof(KnotAudioDataReference))]
    public class KnotAudioDataReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_provider"), label, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_provider"), true);
        }
    }
}
