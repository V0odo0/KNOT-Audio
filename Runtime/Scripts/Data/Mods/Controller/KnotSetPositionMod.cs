using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Set Position", menuCustomName: "Controller/Set Position", order: 1000)]
    public class KnotSetPositionMod : IKnotControllerMod
    {
        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }
        [SerializeField] private Vector3 _position;

        public Space Space
        {
            get => _space;
            set => _space = value;
        }
        [SerializeField] private Space _space = Space.Self;


        public KnotSetPositionMod() { }

        public KnotSetPositionMod(Vector3 position, Space space = Space.Self)
        {
            _position = position;
            _space = space;
        }


        public void Setup(KnotAudioControllerBase controller)
        {
            if (Space == Space.Self)
                controller.transform.localPosition = Position;
            else controller.transform.position = Position;
        }
    }
}
