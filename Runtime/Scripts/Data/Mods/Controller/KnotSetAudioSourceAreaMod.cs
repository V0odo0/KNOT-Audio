using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Set AudioSource Area", menuCustomName: "Controller/Set AudioSource Area", order: 1000)]
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


        public void Setup(KnotAudioControllerBase controller)
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
