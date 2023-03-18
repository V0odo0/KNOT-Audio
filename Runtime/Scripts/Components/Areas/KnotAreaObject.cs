using System;
using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAreaObject : MonoBehaviour
    {
        public List<IKnotVolumeSource> VolumeSources =>
            _volumeSources ?? (_volumeSources = new List<IKnotVolumeSource>());
        [SerializeReference, KnotTypePicker(typeof(IKnotVolumeSource))]
        private List<IKnotVolumeSource> _volumeSources;
        
        public float BlendDistance
        {
            get => Mathf.Clamp(_blendDistance, 0, float.MaxValue);
            set => _blendDistance = value;
        }
        [SerializeField, Min(0)] private float _blendDistance;


        public virtual (Vector3 closestPoint, float weight) SampleAllVolumes(Vector3 pos, float maxDistance = 0)
        {
            if (VolumeSources.Count == 0)
                return (pos, 0);

            (Vector3 closestPoint, float weight) closestSample = (pos, -1f);

            for (int i = 0; i < VolumeSources.Count; i++)
            {
                if (VolumeSources[i] == null)
                    continue;

                if (i == 0)
                    closestSample = VolumeSources[i].Sample(pos, BlendDistance);
                else
                {
                    var sample = VolumeSources[i].Sample(pos, BlendDistance);
                    if (sample.weight > closestSample.weight)
                        closestSample = sample;
                }
            }
            foreach (var vs in VolumeSources)
            {
                if (vs == null)
                    continue;
            }

            return closestSample;
        }


        protected virtual void OnDrawGizmosSelected()
        {
            foreach (var vs in VolumeSources)
            {
                if (vs == null)
                    return;

                vs.DrawGizmos();
            }
        }


        public readonly struct SampleResult
        {
            public readonly Vector3 Position;
            public readonly float Weight;
            
            public SampleResult(Vector3 position, float weight)
            {
                Position = position;
                Weight = weight;
            }
        }
        
        
    }
}
