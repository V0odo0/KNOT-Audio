using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Set Position")]
    public class KnotSetPositionMod : IKnotAudioSourceMod
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


        public void Initialize(KnotAudioSourceController sourceController)
        {
            if (Space == Space.Self)
                sourceController.transform.localPosition = Position;
            else sourceController.transform.position = Position;
        }
    }
}
