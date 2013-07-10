using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Events;

namespace Project2.Entity
{
    class Ring:Entity
    {
        public Ring(Game game, Vector3 position)
            :base(game)
        {
            addComponent(new Spatial(game, this, position));
            getComponent<Spatial>().transform = Matrix.CreateScale(100.0f) * Matrix.CreateRotationX((float)Math.PI/2) * Matrix.CreateTranslation(position);
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"Models\Ring2")));
        }
    }
}
