using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Component
{
    class Spatial2D:Component
    {
        public Vector2 position { get; set; }
        public float rotation { get; set; }
        public Vector2 scale { get; set; }

        public Spatial2D(Game game, Entity.Entity parent, Vector2 position, Vector2 scale, float rotation = 0.0f)
            : base(game, parent)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}
