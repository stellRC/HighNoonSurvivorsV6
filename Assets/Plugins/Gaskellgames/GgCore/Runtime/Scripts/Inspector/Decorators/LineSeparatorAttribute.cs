using System;
using UnityEngine;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class LineSeparatorAttribute : PropertyAttribute
    {
        public float thickness;
        public bool spacingBefore;
        public bool spacingAfter;
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public LineSeparatorAttribute()
        {
            thickness = 1;
            spacingBefore = true;
            spacingAfter = true;
            R = 179;
            G = 179;
            B = 179;
            A = 255;
        }

        public LineSeparatorAttribute(float thickness = 1.0f)
        {
            this.thickness = thickness;
            spacingBefore = true;
            spacingAfter = true;
            R = 179;
            G = 179;
            B = 179;
            A = 255;
        }

        public LineSeparatorAttribute(bool spacingBefore, bool spacingAfter = true)
        {
            thickness = 1;
            this.spacingBefore = spacingBefore;
            this.spacingAfter = spacingAfter;
            R = 179;
            G = 179;
            B = 179;
            A = 255;
        }

        public LineSeparatorAttribute(float thickness, bool spacingBefore, bool spacingAfter = true)
        {
            this.thickness = thickness;
            this.spacingBefore = spacingBefore;
            this.spacingAfter = spacingAfter;
            R = 179;
            G = 179;
            B = 179;
            A = 255;
        }

        public LineSeparatorAttribute(float thickness, byte r, byte g, byte b, byte a = 255)
        {
            this.thickness = thickness;
            spacingBefore = true;
            spacingAfter = true;
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public LineSeparatorAttribute(byte r, byte g, byte b, byte a = 255)
        {
            thickness = 1;
            spacingBefore = true;
            spacingAfter = true;
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public LineSeparatorAttribute(float thickness, bool spacingBefore, bool spacingAfter, byte r, byte g, byte b, byte a = 255)
        {
            this.thickness = thickness;
            this.spacingBefore = spacingBefore;
            this.spacingAfter = spacingAfter;
            R = r;
            G = g;
            B = b;
            A = a;
        }
        
    } // class end
}
