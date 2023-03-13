using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Library Entry", order: -1000)]
    public class KnotLibraryAudioDataProvider : IKnotAudioDataProvider
    {
        public string EntryName
        {
            get => _entryName;
            set => _entryName = value;
        }
        [SerializeField] private string _entryName;

        public IKnotAudioData AudioData => KnotAudio.GetLibraryEntry(EntryName);


        public KnotLibraryAudioDataProvider() { }

        public KnotLibraryAudioDataProvider(string entryName)
        {
            _entryName = entryName;
        }
    }
}
