using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    [DisallowMultipleComponent]
    [AddComponentMenu(KnotAudio.CorePath + "Audio Reverb Zone Area", 1000)]
    public class KnotAudioReverbZoneArea : KnotAreaObject
    {
        public List<AudioReverbZone> ReverbZones => _reverbZones ?? (_reverbZones = new List<AudioReverbZone>());
        [SerializeField] private List<AudioReverbZone> _reverbZones;

        
        protected virtual void Update()
        {
            UpdateReverbZones();
        }

        protected virtual void UpdateReverbZones()
        {
            if (KnotAudio.AudioListener == null)
                return;

            var sample = SampleAllVolumes(KnotAudio.AudioListener.position, BlendDistance);
            foreach (var reverbZone in ReverbZones)
            {
                if (reverbZone == null)
                    continue;

                reverbZone.transform.position = Mathf.Approximately(sample.weight, 1)
                    ? KnotAudio.AudioListener.position
                    : sample.closestPoint;
            }
        }
    }
}
