using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotVolumeSource
    {
        (Vector3 closestPoint, float weight) Sample(Vector3 pos, float maxDistance = 0f);
        
        Bounds CalculateWorldBounds(float maxExpandDistance);

        void DrawGizmos();
    }
}
