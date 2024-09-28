using System.Collections.Generic;
using System.Linq;
using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotTrackedVolumeObject<T> : KnotTrackedMonoBehaviour<T> where T : MonoBehaviour
    {
        public List<IKnotVolumeSource> VolumeSources =>
            _volumeSources ?? (_volumeSources = new List<IKnotVolumeSource>());
        [SerializeReference, KnotTypePicker(typeof(IKnotVolumeSource))]
        private List<IKnotVolumeSource> _volumeSources;

        public KnotVolumeBoundsUpdateMode BoundsUpdateMode
        {
            get => _boundsUpdateMode;
            set => _boundsUpdateMode = value;
        }
        [SerializeField] private KnotVolumeBoundsUpdateMode _boundsUpdateMode = KnotVolumeBoundsUpdateMode.EveryFrame;
        
        public float BlendDistance
        {
            get => Mathf.Clamp(_blendDistance, 0, float.MaxValue);
            set => _blendDistance = value;
        }
        [SerializeField, Min(0)] private float _blendDistance;


        private Bounds _currentWorldBounds;


        protected override void Awake()
        {
            base.Awake();

            if (BoundsUpdateMode == KnotVolumeBoundsUpdateMode.OnceOnAwake)
                UpdateWorldBounds();
        }

        protected virtual void Update()
        {
            if (BoundsUpdateMode == KnotVolumeBoundsUpdateMode.EveryFrame)
                UpdateWorldBounds();
        }


        public virtual float GetWeight(Vector3 atPosition)
        {
            if (!enabled || VolumeSources.Count == 0 || !_currentWorldBounds.Contains(atPosition))
                return 0;

            var maxWeight = VolumeSources.Where(s => s != null).Select(s => s.Sample(atPosition, BlendDistance)).Max(t => t.weight);
            return maxWeight;
        }

        public virtual void UpdateWorldBounds()
        {
            var maxExpandDst = GetMaxBoundsExpandDistance();

            _currentWorldBounds = VolumeSources.FirstOrDefault(s => s != null)?.CalculateWorldBounds(maxExpandDst) ?? new Bounds();
            foreach (var vs in VolumeSources.Where(s => s != null))
            {
                var b = vs.CalculateWorldBounds(maxExpandDst);
                if (b == default)
                    continue;

                _currentWorldBounds.Encapsulate(b);
            }
        }

        protected virtual float GetMaxBoundsExpandDistance() => BlendDistance;


        protected virtual void OnDrawGizmosSelected()
        {
            if (VolumeSources.Count > 0)
            {
                foreach (var vs in VolumeSources)
                    vs?.DrawGizmos();

                if (!Application.isPlaying)
                    UpdateWorldBounds();

                Gizmos.color = new Color(1, 1, 0, 0.5f);
                Gizmos.DrawWireCube(_currentWorldBounds.center, _currentWorldBounds.size);
                Gizmos.color = Color.white;
            }
        }
    }
}
