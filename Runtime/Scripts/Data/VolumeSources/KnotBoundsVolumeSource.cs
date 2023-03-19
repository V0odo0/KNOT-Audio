using System;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Bounds")]
    public class KnotBoundsVolumeSource : IKnotVolumeSource
    {
        public Transform Pivot
        {
            get => _pivot;
            set => _pivot = value;
        }
        [SerializeField] private Transform _pivot;
        
        public List<Bounds> Bounds => _bounds ?? (_bounds = new List<Bounds>());
        [SerializeField] private List<Bounds> _bounds;
        

        public KnotBoundsVolumeSource() { }

        public KnotBoundsVolumeSource(Transform pivot = null, params Bounds[] bounds)
        {
            Pivot = pivot;
            Bounds.AddRange(bounds);
        }


        public (Vector3 closestPoint, float weight) Sample(Vector3 pos, float maxDistance = 0)
        {
            if (Bounds.Count == 0)
                return (pos, 0);

            float outDst = float.PositiveInfinity;
            Vector3 closestPoint = pos;
            foreach (var bound in Bounds)
            {
                var cp = (Pivot == null ? bound : new Bounds(bound.center + Pivot.position, bound.size)).ClosestPoint(pos);
                var d = (cp - pos).sqrMagnitude;
                if (d < outDst)
                {
                    outDst = d;
                    closestPoint = cp;
                }
            }

            var weight = Mathf.Approximately(outDst, 0) ? 1 : Mathf.Clamp01((maxDistance - outDst) / maxDistance);
            return (closestPoint, weight);
        }

        public Bounds CalculateWorldBounds(float maxExpandDistance)
        {
            var bounds = new Bounds
            {
                min = Bounds.Min(b => b.min),
                max = Bounds.Max(b => b.max)
            };
            foreach (var b in Bounds)
                bounds.Encapsulate(b);

            bounds.Expand(maxExpandDistance);
            return bounds;
        }

        public void DrawGizmos()
        {
            if (Bounds.Count == 0)
                return;

            Gizmos.color = new Color(1, 1, 0, 0.2f);
            foreach (var b in Bounds)
            {
                var localBound = Pivot == null ? b : new Bounds(Pivot.TransformPoint(b.center), b.size);
                Gizmos.DrawCube(localBound.center, localBound.size);
                Gizmos.DrawWireCube(localBound.center, localBound.size);
            }
            Gizmos.color = Color.white;
        }
    }
}
