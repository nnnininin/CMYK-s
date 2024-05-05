using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Util
{
    public class NamedArrayAttribute : PropertyAttribute
    {
        public readonly string[] Names;
        public NamedArrayAttribute(string[] names) { Names = names; }
    }
}