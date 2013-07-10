using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project2
{
    class PivotTest:CurvePath
    {
        float width;
        float height;
        Vector2 lastGoodTangent;

        public PivotTest(float width, float height)
        {
            this.width = width;
            this.height = height;
            lastGoodTangent = new Vector2(0, 1);
        }

        public Vector2 eval(float t)
        {
            return new Vector2((float)Math.Sin(t) + width,(float)Math.Cos(t) + height);
        }

        public Vector2 derivative(float t)
        {
            return new Vector2((float)-Math.Cos(t),(float)-Math.Sin(t));
        }

        public Vector2 derivativeNZ(float t)
        {
            Vector2 deriv = derivative(t);
            if (deriv != Vector2.Zero)
            {
                lastGoodTangent = deriv;
                return deriv;
            }
            else
            {
                return lastGoodTangent;
            };
        }
    }
}
