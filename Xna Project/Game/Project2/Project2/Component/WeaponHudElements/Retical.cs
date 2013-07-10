using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;

namespace Project2.Component
{
    class Retical:Entity.Entity
    {
        public Retical(Game game)
            : base(game)
        {
            addComponent(new Spatial2D(game, this, Vector2.Zero, new Vector2(1, 1)));
            addComponent(new Drawable2D(game, this, Game.Content.Load<Texture2D>(@"Textures/Retical_02"),Color.Lime));
        }
    }
}
