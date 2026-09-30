using System;
using System.Linq;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    /// <summary>
    /// Swaps the played clip for a distant variant when the controller is at least <see cref="MinDistance"/> away from the <see cref="KnotAudio.AudioListener"/>.
    /// Reads the controller position during Setup, so place it after any mod that moves the controller (Set Parent, Set Position)
    /// </summary>
    [Serializable]
    [KnotTypeInfo(displayName: "Distant Variant", Order = -975)]
    public class KnotDistantVariantMod : IKnotControllerMod, IKnotAudioGroupMod
    {
        public float MinDistance
        {
            get => Mathf.Max(_minDistance, 0);
            set => _minDistance = value;
        }
        [SerializeField, Min(0)] private float _minDistance = 50f;

        public KnotAudioDataReference DistantAudioData
        {
            get => _distantAudioData ?? (_distantAudioData = new KnotAudioDataReference());
            set => _distantAudioData = value;
        }
        [SerializeField] private KnotAudioDataReference _distantAudioData;

        /// <summary>
        /// Whether the distant variant's own Data Mods are applied to the controller after the clip swap
        /// </summary>
        public bool ApplyDistantMods
        {
            get => _applyDistantMods;
            set => _applyDistantMods = value;
        }
        [SerializeField] private bool _applyDistantMods = true;


        public KnotDistantVariantMod() { }

        public KnotDistantVariantMod(float minDistance, IKnotAudioDataProvider distantAudioDataProvider, bool applyDistantMods = true)
        {
            _minDistance = minDistance;
            _distantAudioData = new KnotAudioDataReference { Provider = distantAudioDataProvider };
            _applyDistantMods = applyDistantMods;
        }


        public bool IsDistant(KnotAudioControllerBase controller)
        {
            var listener = KnotAudio.AudioListener;
            if (controller == null || listener == null)
                return false;

            return Vector3.Distance(listener.position, controller.transform.position) >= MinDistance;
        }

        public void Setup(KnotAudioControllerBase controller)
        {
            if (controller == null || !IsDistant(controller))
                return;

            var distantData = DistantAudioData.Provider?.AudioData;
            if (distantData == null || distantData.AudioClip == null)
                return;

            controller.AudioSource.clip = distantData.AudioClip;

            if (!ApplyDistantMods)
                return;

            //Nested Distant Variant mods are skipped to prevent recursion
            foreach (var mod in distantData.GetAllMods().Where(m => m != null && !(m is KnotDistantVariantMod)))
                mod.Setup(controller);
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithDistantVariant(float minDistance, IKnotAudioDataProvider distantAudioDataProvider, bool applyDistantMods = true)
        {
            if (Controller != null && distantAudioDataProvider != null)
                Controller.AppendMods(new KnotDistantVariantMod(minDistance, distantAudioDataProvider, applyDistantMods));

            return this;
        }
    }
}
