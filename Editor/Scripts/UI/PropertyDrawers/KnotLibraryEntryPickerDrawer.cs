using Knot.Audio.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomPropertyDrawer(typeof(KnotLibraryEntryNamePickerAttribute))]
    public class KnotLibraryEntryPickerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (KnotAudio.SettingsProfile == null || property.propertyType != SerializedPropertyType.String)
            {
                base.OnGUI(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label);

            Rect popupPos = new Rect(position.x + EditorGUIUtility.labelWidth, position.y,
                position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            if (EditorGUI.DropdownButton(popupPos, EditorGUIUtility.TrTextContent(string.IsNullOrEmpty(property.stringValue) ? "[None]" : property.stringValue),
                    FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                foreach (var entryName in KnotAudioSettingsProfile.CachedLibraryEntryNames)
                {
                    bool isSelected = entryName == property.stringValue;
                    menu.AddItem(EditorGUIUtility.TrTextContent(entryName), isSelected, () =>
                    {
                        property.stringValue = entryName;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.DropDown(popupPos);
            }

            EditorGUI.EndProperty();
        }
    }
}
