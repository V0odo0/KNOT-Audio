using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotVariantSelection<T> where T : class
    {
        protected static List<T> VariantsTemp { get; } = new List<T>();


        public virtual SelectionMethod Selection
        {
            get => _selection;
            set => _selection = value;
        }
        [SerializeField] protected SelectionMethod _selection;

        public virtual List<T> Variants => _variants ?? (_variants = new List<T>());
        [SerializeField] protected List<T> _variants;


        [NonSerialized] protected T _lastSelectedVariant;
        

        public virtual T SelectNext(T fallback = default)
        {
            if (Variants.Count == 0)
                return fallback;

            switch (Selection)
            {
                case SelectionMethod.Random:
                    _lastSelectedVariant = Variants[UnityEngine.Random.Range(0, Variants.Count)];
                    break;
                case SelectionMethod.RandomNoRepeat:
                    VariantsTemp.Clear();
                    foreach (var variant in Variants)
                    {
                        if (variant == null || variant == _lastSelectedVariant)
                            continue;

                        VariantsTemp.Add(variant);
                    }

                    if (VariantsTemp.Count != 0)
                        _lastSelectedVariant = VariantsTemp[UnityEngine.Random.Range(0, VariantsTemp.Count)];
                    break;
                case SelectionMethod.Sequence:
                    if (_lastSelectedVariant == null)
                        _lastSelectedVariant = Variants[0];
                    else
                    {
                        var lastId = Variants.IndexOf(_lastSelectedVariant);
                        if (lastId >= Variants.Count - 1)
                            lastId = 0;
                        else lastId++;

                        _lastSelectedVariant = Variants[lastId];
                    }
                    break;
            }

            return _lastSelectedVariant;
        }

        [Serializable]
        public enum SelectionMethod
        {
            Random,
            RandomNoRepeat,
            Sequence
        }
    }
}
