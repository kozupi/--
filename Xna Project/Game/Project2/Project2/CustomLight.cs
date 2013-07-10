﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Storage;

namespace Project2
{
    public class CustomLight
    {
        public Vector3 AmbientLightColor { get; set; }
        public Vector3 LightPosition { get; set; }
        public Vector3 LightColor { get; set; }
        public float LightOffset { get; set; }
        public float LightDrop { get; set; }

        public CustomLight()
        {
            AmbientLightColor = new Vector3(.15f, .15f, .15f);
            LightPosition = new Vector3(0, 0, 0);
            LightColor = new Vector3(.85f, .85f, .85f);
            LightOffset = 5000;
            LightDrop = 2;
        }

        public void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["AmbientLightColor"] != null)
                effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);

            if (effect.Parameters["LightPosition"] != null)
                effect.Parameters["LightPosition"].SetValue(LightPosition);

            if (effect.Parameters["LightColor"] != null)
                effect.Parameters["LightColor"].SetValue(LightColor);

            if (effect.Parameters["LightAttenuation"] != null)
                effect.Parameters["LightAttenuation"].SetValue(LightOffset);

            if (effect.Parameters["LightFalloff"] != null)
                effect.Parameters["LightFalloff"].SetValue(LightDrop);
        }
    }
}
