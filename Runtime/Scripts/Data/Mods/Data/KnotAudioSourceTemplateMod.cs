using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName:"AudioSource Template")]
    public class KnotAudioSourceTemplateMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public AudioSource Template
        {
            get => _template;
            set => _template = value;
        }
        [SerializeField] private AudioSource _template;


        public KnotAudioSourceTemplateMod() { }

        public KnotAudioSourceTemplateMod(AudioSource template)
        {
            _template = template;
        }

        public void Setup(KnotAudioControllerBase controller)
        {
            
        }

        /*
        public void Setup(KnotAudioController controller)
        {
            if (Template == null)
                return;

            var from = Template;
            var to = controller.AudioSource;

            to.outputAudioMixerGroup = from.outputAudioMixerGroup;
            to.mute = from.mute;
            to.bypassEffects = from.bypassEffects;
            to.bypassListenerEffects = from.bypassListenerEffects;
            to.bypassReverbZones = from.bypassReverbZones;
            to.priority = from.priority;
            to.volume = from.volume;
            to.pitch = from.pitch;
            to.panStereo = from.panStereo;
            to.spatialBlend = from.spatialBlend;
            to.reverbZoneMix = from.reverbZoneMix;

            to.dopplerLevel = from.dopplerLevel;
            to.spread = from.spread;
            to.rolloffMode = from.rolloffMode;
            to.minDistance = from.minDistance;
            to.maxDistance = from.maxDistance;

            foreach (AudioSourceCurveType e in Enum.GetValues(typeof(AudioSourceCurveType)))
            {
                try
                {
                    to.SetCustomCurve(e, from.GetCustomCurve(e));
                }
                catch
                {
                    //
                }
            }
        }*/
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle FromTemplate(AudioSource template)
        {
            if (Controller != null && template != null)
                Controller.AppendMods(new KnotAudioSourceTemplateMod(template));

            return this;
        }
    }
}
