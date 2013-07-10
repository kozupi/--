using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Component;
using Microsoft.Xna.Framework.Graphics;
namespace Project2.Entity
{
    class DebugShape : Entity
    {
        Vector3 position;

        public DebugShape(Game game,Model m, Vector3 pos)
            : base(game)
        {
            position = pos;
            addComponent(new Spatial(game, this, pos));
            addComponent(new Drawable3D(game, this, m));
        }
    }
}
