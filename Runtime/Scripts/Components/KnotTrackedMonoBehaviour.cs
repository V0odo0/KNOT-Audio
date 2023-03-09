using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotTrackedMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static IReadOnlyCollection<T> AllInstances => _allInstances ?? (_allInstances = new HashSet<T>());
        private static HashSet<T> _allInstances = new HashSet<T>();

        public static IReadOnlyCollection<T> ActiveInstances => _activeInstances ?? (_activeInstances = new HashSet<T>());
        private static HashSet<T> _activeInstances = new HashSet<T>();


        protected virtual void Awake()
        {
            if (this is T)
                _allInstances.Add(this as T);
        }

        protected virtual void OnEnable()
        {
            if (this is T)
                _activeInstances.Add(this as T);
        }

        protected virtual void OnDisable()
        {
            if (this is T)
                _activeInstances.Remove(this as T);
        }

        protected virtual void OnDestroy()
        {
            if (this is T)
                _allInstances.Remove(this as T);
        }
    }
}
