using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/AudioMixer Snapshot Volume", 1000)]
    public class KnotAudioMixerSnapshotVolume : KnotTrackedVolumeObject<KnotAudioMixerSnapshotVolume>
    {
        public AudioMixerSnapshot Snapshot
        {
            get => _snapshot;
            set => _snapshot = value;
        }
        [SerializeField] private AudioMixerSnapshot _snapshot;


        public override float GetWeight(Vector3 atPosition) => Snapshot == null ? 0 : Mathf.Clamp(base.GetWeight(atPosition), 0, MaxWeight);
    }
}
