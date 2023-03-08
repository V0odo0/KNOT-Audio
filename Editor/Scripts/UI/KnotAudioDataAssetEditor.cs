using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Knot.Audio.Editor
{
    [CustomEditor(typeof(KnotAudioDataAsset))]
    [CanEditMultipleObjects]
    public class KnotAudioDataAssetEditor : UnityEditor.Editor
    {
        private SerializedProperty _dataProperty;
        private KnotAudioDataAsset _target;


        void OnEnable()
        {
            _target = target as KnotAudioDataAsset;
            _dataProperty = serializedObject.FindProperty("_data");

        }

        void OnDisable()
        {
            KnotEditorUtils.StopAllPreviewClips();
        }

        public override void OnInspectorGUI()
        {
            if (_dataProperty == null)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(_dataProperty.FindPropertyRelative("_clip"), true);
            EditorGUILayout.PropertyField(_dataProperty.FindPropertyRelative("_mods"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

