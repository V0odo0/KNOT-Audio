using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Set AudioSource Area")]
    public class KnotSetAudioSourceAreaMod : IKnotControllerMod
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


        public void Setup(KnotAudioController controller)
        {
            if (Area == null)
                return;

            Area.Attach(controller.AudioSource);
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle AtArea(KnotAudioSourceArea area)
        {
            if (Controller != null && area != null)
                Controller.AppendMods(new KnotSetAudioSourceAreaMod(area));

            return this;
        }
    }
}
