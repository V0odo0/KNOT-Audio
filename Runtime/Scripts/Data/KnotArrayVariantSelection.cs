using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace Knot.Audio
{
    public abstract class KnotArrayVariantSelection<T> where T : class
    {
        protected static List<T> VariantsTemp { get; } = new List<T>();


        public virtual SelectionMethod Method
        {
            get => _method;
            set => _method = value;
        }
        [SerializeField] protected SelectionMethod _method;

        public virtual List<T> Variants => _variants ?? (_variants = new List<T>());
        [SerializeField] protected List<T> _variants;


        [NonSerialized] protected T _lastSelectedVariant;
        

        public virtual T SelectNext()
        {
            if (Variants.Count == 0)
                return default;

            switch (Method)
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
