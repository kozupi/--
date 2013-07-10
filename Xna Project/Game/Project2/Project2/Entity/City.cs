using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;

namespace Project2.Entity
{
    class City:Entity
    {
        public City(Game game, Vector3 position,float size)
            :base(game)
        {
            addComponent(new Spatial(game, this, Vector3.Zero));
            getComponent<Spatial>().transform = Matrix.CreateScale(size) * Matrix.CreateTranslation(position); ;
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"Models\TheCity")));
        }
    }
}
