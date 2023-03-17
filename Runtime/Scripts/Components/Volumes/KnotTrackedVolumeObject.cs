using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotTrackedVolumeObject<T> : KnotTrackedMonoBehaviour<T> where T : MonoBehaviour
    {
        public List<IKnotVolumeSource> VolumeSources =>
            _volumeSources ?? (_volumeSources = new List<IKnotVolumeSource>());
        [SerializeReference, KnotTypePicker(typeof(IKnotVolumeSource))]
        private List<IKnotVolumeSource> _volumeSources;

        public float MaxWeight
        {
            get => Mathf.Clamp01(_maxWeight);
            set => _maxWeight = value;
        }
        [SerializeField, Range(0f, 1f)] private float _maxWeight = 1f;

        public float BlendDistance
        {
            get => Mathf.Clamp(_blendDistance, 0, float.MaxValue);
            set => _blendDistance = value;
        }
        [SerializeField, Min(0)] private float _blendDistance;

        public bool InverseVolume
        {
            get => _inverseVolume;
            set => _inverseVolume = value;
        }
        [SerializeField] private bool _inverseVolume;


        public virtual float GetWeight(Vector3 atPosition)
        {
            if (!enabled || VolumeSources.Count == 0)
                return InverseVolume ? 1 : 0;

            var maxWeight = VolumeSources.Where(s => s != null).Select(s => s.Sample(atPosition, BlendDistance))
                .Max(t => t.weight);
            return InverseVolume ? 1 - maxWeight : maxWeight;
        }


        protected virtual void OnDrawGizmosSelected()
        {
            if (VolumeSources.Count > 0)
            {
                Gizmos.color = new Color(0, 1, 0, 0.1f);
                foreach (var bound in VolumeSources)
                {
                    if (bound is KnotBoundsVolumeSource bvs)
                        bvs.DrawGizmos();
                }
                Gizmos.color = Color.white;
            }
        }
    }
}
