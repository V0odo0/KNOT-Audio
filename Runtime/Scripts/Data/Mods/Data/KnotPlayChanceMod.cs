using System;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Play Chance", Order = -990)]
    public class KnotPlayChanceMod : IKnotAudioDataMod
    {
        public float Chance
        {
            get => Mathf.Clamp01(_chance);
            set => _chance = value;
        }
        [SerializeField, Range(0f, 1f)] private float _chance = 0.5f;


        public KnotPlayChanceMod() { }

        public KnotPlayChanceMod(float chance)
        {
            _chance = chance;
        }


        public bool SampleCanPlay() => UnityEngine.Random.value >= Chance;

        public void Setup(KnotAudioControllerBase controller) { }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle WithPlayChance(float chance)
        {
            if (Controller != null)
                Controller.AppendMods(new KnotPlayChanceMod(chance));

            return this;
        }
    }
}
