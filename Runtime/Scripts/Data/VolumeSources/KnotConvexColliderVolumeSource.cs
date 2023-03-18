using System;
using Knot.Audio.Attributes;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo("Convex Collider")]
    public class KnotConvexColliderVolumeSource : IKnotVolumeSource
    {
        public Collider Collider
        {
            get => _collider;
            set => _collider = value;
        }
        [SerializeField] private Collider _collider;


        public KnotConvexColliderVolumeSource() { }

        public KnotConvexColliderVolumeSource(Collider convexCollider)
        {
            _collider = convexCollider;
        }


        public (Vector3 closestPoint, float weight) Sample(Vector3 pos, float maxDistance = 0f)
        {
            if (Collider == null || !Collider.enabled || Collider is MeshCollider { convex: false })
                return (pos, 0);

            float outDst = float.PositiveInfinity;
            var closestPoint = Collider.ClosestPoint(pos);
            var d = (closestPoint - pos).sqrMagnitude;
            if (d < outDst)
                outDst = d;
            
            var weight = Mathf.Approximately(outDst, 0) ? 1 : Mathf.Clamp01((maxDistance - outDst) / maxDistance);
            return (closestPoint, weight);
        }

        public void DrawGizmos()
        {
            if (Collider == null || !Collider.enabled || Collider is MeshCollider { convex: false })
                return;

            Gizmos.color = new Color(0, 1, 0, 0.1f);
            if (Collider is SphereCollider sc)
            {
                Gizmos.DrawSphere(sc.transform.position, sc.radius * sc.transform.lossyScale.magnitude);
            }
            else if (Collider is MeshCollider mc && mc.sharedMesh != null)
            {
                Gizmos.DrawMesh(mc.sharedMesh, mc.transform.position, mc.transform.rotation, mc.transform.lossyScale);
            }

            Gizmos.color = Color.white;
        }
    }
}
