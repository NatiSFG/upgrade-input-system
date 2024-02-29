using System;
using UnityEngine;

namespace Game {
    /// <summary>
    /// Marks a field as being read-only in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyFieldAttribute : PropertyAttribute {

    }
}