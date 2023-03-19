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

        public KnotVolumeBoundsUpdateMode BoundsUpdateMode
        {
            get => _boundsUpdateMode;
            set => _boundsUpdateMode = value;
        }
        [SerializeField] private KnotVolumeBoundsUpdateMode _boundsUpdateMode = KnotVolumeBoundsUpdateMode.EveryFrame;

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
                return InverseVolume ? 1 : 0;

            var maxWeight = VolumeSources.Where(s => s != null).Select(s => s.Sample(atPosition, BlendDistance))
                .Max(t => t.weight);
            return InverseVolume ? 1 - maxWeight : maxWeight;
        }

        public virtual void UpdateWorldBounds()
        {
            var maxExpandDst = GetMaxBoundsExpandDistance();

            _currentWorldBounds = new Bounds();
            foreach (var vs in VolumeSources)
            {
                if (vs == null)
                    continue;

                _currentWorldBounds.Encapsulate(vs.CalculateWorldBounds(maxExpandDst));
            }
        }

        protected virtual float GetMaxBoundsExpandDistance() => BlendDistance;


        protected virtual void OnDrawGizmosSelected()
        {
            if (VolumeSources.Count > 0)
            {
                foreach (var vs in VolumeSources)
                    vs?.DrawGizmos();

                Gizmos.color = new Color(1, 1, 0, 0.5f);
                Gizmos.DrawWireCube(_currentWorldBounds.center, _currentWorldBounds.size);
                Gizmos.color = Color.white;
            }
        }
    }
}
