using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/AudioMixer Parameters Volume", 1000)]
    public class KnotAudioMixerParametersVolume : KnotTrackedVolumeObject<KnotAudioMixerParametersVolume>
    {
        public List<Parameter> Parameters => _parameters ?? (_parameters = new List<Parameter>());
        [SerializeField] private List<Parameter> _parameters;


        public override float GetWeight(Vector3 atPosition) => Parameters.Count == 0 ? 0 : Mathf.Clamp(base.GetWeight(atPosition), 0, MaxWeight);
        

        [Serializable]
        public class Parameter
        {
            public string Name
            {
                get => _name;
                set => _name = value;
            }
            [SerializeField] private string _name;

            public float TargetValue
            {
                get => _targetValue;
                set => _targetValue = value;
            }
            [SerializeField] private float _targetValue;
        }
    }
}
