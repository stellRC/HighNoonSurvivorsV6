using System;
using UnityEngine;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames: https://github.com/Gaskellgames
    /// </summary>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UnitAttribute : PropertyAttribute
    {
        public Units unit;
        
        public UnitAttribute(Units unit)
        {
            this.unit = unit;
        }
        
    } // class end
}