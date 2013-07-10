using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project2
{
    interface CurvePath
    {
        Vector2 eval(float t);
        Vector2 derivative(float t);
        Vector2 derivativeNZ(float t);
    }
}
