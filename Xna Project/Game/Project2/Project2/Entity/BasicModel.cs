using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;

namespace Project2.Entity
{
    class BasicModel:Entity
    {

        public BasicModel(Game game, Model m, Matrix transform)
            :base(game)
        {
            addComponent(new Spatial(game, this, Vector3.Zero));
            getComponent<Spatial>().transform = transform;
            addComponent(new Drawable3D(game, this, m));
        }
    }
}
