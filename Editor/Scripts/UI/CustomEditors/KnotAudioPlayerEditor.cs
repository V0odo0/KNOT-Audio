using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomEditor(typeof(KnotAudioPlayer))]
    [CanEditMultipleObjects]
    public class KnotAudioPlayerEditor : UnityEditor.Editor
    {
        private KnotAudioPlayer _target;

        void OnEnable()
        {
            _target = target as KnotAudioPlayer;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_target == null)
                return;

            EditorGUI.BeginDisabledGroup(!Application.isPlaying || _target.AudioDataProviders.Count == 0);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(Application.isPlaying ? "Play Once" : "Play Once [PlayMode only]"))
                _target.Play();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
    }
}
