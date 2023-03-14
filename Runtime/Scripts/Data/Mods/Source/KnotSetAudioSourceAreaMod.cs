using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Set AudioSource Area")]
    public class KnotSetAudioSourceAreaMod : IKnotAudioSourceMod
    {
        public KnotAudioSourceArea Area
        {
            get => _area;
            set => _area = value;
        }
        [SerializeField] private KnotAudioSourceArea _area;


        public KnotSetAudioSourceAreaMod() { }

        public KnotSetAudioSourceAreaMod(KnotAudioSourceArea area)
        {
            _area = area;
        }


        public void Initialize(KnotAudioSourceController sourceController)
        {
            if (Area == null)
                return;

            Area.Attach(sourceController.AudioSource);
        }
    }
}
