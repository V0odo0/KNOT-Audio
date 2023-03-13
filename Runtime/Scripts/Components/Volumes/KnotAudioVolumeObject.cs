using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioVolumeObject<T> : KnotTrackedMonoBehaviour<T> where T : MonoBehaviour
    {
        public List<Collider> VolumeColliders => _volumeColliders ?? (_volumeColliders = new List<Collider>());
        [SerializeField] private List<Collider> _volumeColliders;

        public List<Bounds> VolumeBounds => _volumeBounds ?? (_volumeBounds = new List<Bounds>());
        [SerializeField] private List<Bounds> _volumeBounds;

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
        [SerializeField, Min(0)] private float _blendDistance = 0.1f;


        public virtual float GetWeight(Vector3 atPosition)
        {
            if (!enabled || VolumeColliders.Count == 0)
                return 0;

            float outDst = float.PositiveInfinity;
            foreach (var col in VolumeColliders)
            {
                if (col == null || !col.enabled || col is MeshCollider { convex: false })
                    continue;

                var closestPoint = col.ClosestPoint(atPosition);
                var d = (closestPoint - atPosition).sqrMagnitude;
                if (d < outDst)
                    outDst = d;
            }

            foreach (var bound in VolumeBounds)
            {
                var localBound = new Bounds(transform.TransformPoint(bound.center), bound.size);

                var closestPoint = localBound.ClosestPoint(atPosition);
                var d = (closestPoint - atPosition).sqrMagnitude;
                if (d < outDst)
                    outDst = d;
            }

            float targetWeight;

            if (outDst <= 0)
                targetWeight = MaxWeight;
            else if (outDst < BlendDistance)
                targetWeight = MaxWeight - (MaxWeight * (outDst / BlendDistance));
            else targetWeight = 0;

            return targetWeight;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (VolumeBounds.Count > 0)
            {
                Gizmos.color = new Color(0, 1, 0, 0.1f);
                foreach (var bound in VolumeBounds)
                {
                    var localBound = new Bounds(transform.TransformPoint(bound.center), bound.size);
                    Gizmos.DrawCube(localBound.center, localBound.size);
                    Gizmos.DrawWireCube(localBound.center, localBound.size);
                }
                Gizmos.color = Color.white;
            }
        }
    }
}
