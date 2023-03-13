using UnityEngine;
using UnityEngine.Audio;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/AudioMixer Snapshot Volume", 1000)]
    public class KnotAudioMixerSnapshotVolume : KnotAudioVolumeObject<KnotAudioMixerSnapshotVolume>
    {
        public AudioMixerSnapshot Snapshot
        {
            get => _snapshot;
            set => _snapshot = value;
        }
        [SerializeField] private AudioMixerSnapshot _snapshot;


        public override float GetWeight(Vector3 atPosition) => Snapshot == null ? 0 : base.GetWeight(atPosition);
    }
}
