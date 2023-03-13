using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{

    [Serializable]
    [KnotTypeInfo("3D Sound Settings")]
    public class Knot3DSoundSettingsMod : IKnotAudioDataMod
    {
        public float DopplerLevel
        {
            get => Mathf.Clamp(_dopplerLevel, 0, 5);
            set => _dopplerLevel = value;
        }
        [SerializeField, Range(0, 5)] private float _dopplerLevel = 1;

        public int Spread
        {
            get => Mathf.Clamp(_spread, 0, 360);
            set => _spread = value;
        }
        [SerializeField, Range(0, 360)] private int _spread = 0;

        public float MinDistance
        {
            get => Mathf.Clamp(_minDistance, 0, _maxDistance);
            set => _minDistance = value;
        }
        [SerializeField, Min(0)] private float _minDistance = 1;

        public float MaxDistance
        {
            get => Mathf.Clamp(_maxDistance, _minDistance, float.MaxValue);
            set => _maxDistance = value;
        }
        [SerializeField, Min(0)] private float _maxDistance = 500;
        
        public AudioRolloffMode RolloffMode
        {
            get => _rolloffMode;
            set => _rolloffMode = value;
        }
        [SerializeField] private AudioRolloffMode _rolloffMode = AudioRolloffMode.Logarithmic;


        public Knot3DSoundSettingsMod() { }

        public Knot3DSoundSettingsMod(float minDistance, float maxDistance)
        {
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }

        public Knot3DSoundSettingsMod(float dopplerLevel, int spread, float minDistance, float maxDistance, AudioRolloffMode rolloffMode)
        {
            _dopplerLevel = dopplerLevel;
            _spread = spread;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            _rolloffMode = rolloffMode;
        }


        public void Initialize(KnotAudioSourceController sourceController)
        {
            sourceController.AudioSource.dopplerLevel = DopplerLevel;
            sourceController.AudioSource.spread = Spread;
            sourceController.AudioSource.minDistance = MinDistance;
            sourceController.AudioSource.maxDistance = MaxDistance;
            sourceController.AudioSource.rolloffMode = RolloffMode;
        }
    }
}
