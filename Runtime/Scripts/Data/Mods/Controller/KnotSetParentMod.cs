using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Set Parent", menuCustomName:"Controller/Set Parent", order: 1000)]
    public class KnotSetParentMod : IKnotControllerMod
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


        public void Setup(KnotAudioControllerBase controller)
        {
            var parent = Parent != null && Parent.gameObject.scene.IsValid() ? Parent : null;
            controller.transform.SetParent(parent);
            controller.transform.localPosition = Vector3.zero;
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle AttachTo(Transform parent, Vector3 localPos)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotSetParentMod(parent), new KnotSetPositionMod(localPos));

            return this;
        }

        public KnotAudioControllerHandle AttachTo(Transform parent)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotSetParentMod(parent));

            return this;
        }
    }
}
