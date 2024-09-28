using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace Knot.Audio.Editor
{
    internal static class EditorUtils
    {
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


        public static void PlayPreviewClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            MethodInfo method = AudioUtilsType.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new [] {
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