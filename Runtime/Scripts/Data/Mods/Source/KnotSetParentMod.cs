using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Set Parent")]
    public class KnotSetParentMod : IKnotAudioSourceMod
    {
        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }
        [SerializeField] private Transform _parent;


        public KnotSetParentMod() { }

        public KnotSetParentMod(Transform parent)
        {
            _parent = parent;
        }


        public void Initialize(KnotAudioSourceController sourceController)
        {
            sourceController.transform.SetParent(Parent);
            sourceController.transform.localPosition = Vector3.zero;
        }
    }
}
