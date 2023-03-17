using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"Set Parent")]
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


        public void Setup(KnotAudioController controller)
        {
            controller.transform.SetParent(Parent);
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
    }
}
