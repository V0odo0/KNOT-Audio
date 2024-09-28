using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Library Entry", order: -1000)]
    public class KnotLibraryEntryProvider : IKnotAudioDataProvider
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private byte _doNotDrawNameOnArrayElement;
#endif

        public string EntryName
        {
            get => _entryName;
            set => _entryName = value;
        }
        [SerializeField, KnotLibraryEntryNamePicker] private string _entryName;

        public IKnotAudioData AudioData => KnotAudio.GetLibraryEntry(EntryName);


        public KnotLibraryEntryProvider() { }

        public KnotLibraryEntryProvider(string entryName)
        {
            _entryName = entryName;
        }
    }
}
