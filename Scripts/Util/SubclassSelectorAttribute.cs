using System;
using UnityEngine;

namespace Util
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        private readonly bool mIncludeMono;

        public SubclassSelectorAttribute(bool includeMono = false)
        {
            mIncludeMono = includeMono;
        }

        public bool IsIncludeMono()
        {
            return mIncludeMono;
        }
    }
}
