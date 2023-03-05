using System;
using UnityEngine;

namespace Knot.Audio.Attributes
{
    /// <summary>
    /// Special attribute for <see cref="Type"/> dependent types and fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class KnotTypePickerAttribute : PropertyAttribute
    {
        public readonly Type BaseType;
        public readonly bool DrawLabel;


        public KnotTypePickerAttribute(Type baseType, bool drawLabel = true)
        {
            BaseType = baseType;
            DrawLabel = drawLabel;
        }
    }
}