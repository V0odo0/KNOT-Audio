using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CorePath + "Audio Mixer Parameters Volume", 1000)]
    public class KnotAudioMixerParametersVolume : KnotTrackedVolumeObject<KnotAudioMixerParametersVolume>
    {
        public int Priority
        {
            get => _priority;
            set => _priority = value;
        }
        [SerializeField] private int _priority = 0;

        public List<Parameter> Parameters => _parameters ?? (_parameters = new List<Parameter>());
        [SerializeField] private List<Parameter> _parameters;

        
        public override float GetWeight(Vector3 atPosition) => Parameters.Count == 0 ? 0 : Mathf.Clamp01(base.GetWeight(atPosition));
        

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
