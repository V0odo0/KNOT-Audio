using System;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Knot.Audio.Editor
{
    [CustomPropertyDrawer(typeof(KnotTypePickerAttribute))]
    public class KnotTypePickerDrawer : PropertyDrawer
    {
        public KnotTypePickerAttribute Attribute => attribute as KnotTypePickerAttribute;
        public Type BaseType => Attribute?.BaseType;


        bool IsValidProperty(SerializedProperty property)
        {
            return BaseType != null && 
                   property.propertyType == SerializedPropertyType.ManagedReference &&
                   BaseType.GetDerivedTypesInfo().Any();
        }


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
            => property.GetFallbackPropertyGUI();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsValidProperty(property))
            {
                base.OnGUI(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            Type currentType = property.GetManagedReferenceType();
            var types = BaseType.GetDerivedTypesInfo();

            Rect popupPos = position;
            popupPos.height = EditorGUIUtility.singleLineHeight;
            
            EditorGUI.BeginChangeCheck();

            int selectedTypeInfoId = types.Select((info, i) => new {typeInfo = info, Index = i})
                .FirstOrDefault(t => t.typeInfo.Type == currentType)?.Index ?? -1;
            selectedTypeInfoId = Attribute.DrawLabel ? 
                EditorGUI.Popup(popupPos, label, selectedTypeInfoId, types.Select(ti => ti.Content).ToArray()) : 
                EditorGUI.Popup(popupPos, selectedTypeInfoId, types.Select(ti => ti.Content).ToArray());

            if (EditorGUI.EndChangeCheck() && types[selectedTypeInfoId].Type != property.GetManagedReferenceType())
            {
                property.managedReferenceValue = types[selectedTypeInfoId].GetInstance();
                property.serializedObject.ApplyModifiedProperties();
            }

            if (selectedTypeInfoId >= 0)
            {
                position.y += popupPos.height + EditorGUIUtility.standardVerticalSpacing;
                position.height -= popupPos.height;

                EditorGUI.PropertyField(position, property, types[selectedTypeInfoId].Content, true);
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsValidProperty(property))
                return base.GetPropertyHeight(property, label);
            
            return EditorGUI.GetPropertyHeight(property, true) + (property.GetManagedReferenceType() != null ? EditorGUIUtility.singleLineHeight : 0);
        }
    }
}
