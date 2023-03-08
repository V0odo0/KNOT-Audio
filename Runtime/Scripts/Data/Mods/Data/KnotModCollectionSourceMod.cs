using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Mod Collection Source")]
    public class KnotModCollectionSourceMod : IKnotAudioDataMod
    {
        public KnotModCollectionAsset ModCollection
        {
            get => _modCollection;
            set => _modCollection = value;
        }
        [SerializeField] private KnotModCollectionAsset _modCollection;


        public KnotModCollectionSourceMod() { }

        public KnotModCollectionSourceMod(KnotModCollectionAsset modCollection)
        {
            _modCollection = modCollection;
        }


        public void Initialize(KnotAudioSource source)
        {

        }
    }
}
