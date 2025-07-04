using System;
using UnityEngine;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames: https://github.com/Gaskellgames
    /// </summary>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MaxAttribute : PropertyAttribute
    {
        public float max;

        public MaxAttribute(float max)
        {
            this.max = max;
        }

    } // class end
}
