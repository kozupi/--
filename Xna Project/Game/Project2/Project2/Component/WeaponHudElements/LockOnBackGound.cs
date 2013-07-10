using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;

namespace Project2.Component.WeaponHudElements
{
    class LockOnBackGound:Entity.Entity
    {
        public LockOnBackGound(Game game)
            :base(game)
        {            
            addComponent(new Spatial2D(game, this, new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 3 + 40), new Vector2(0.5f)));
            addComponent(new Drawable2D(game, this, Game.Content.Load<Texture2D>(@"Textures/HUD_02"), Color.White));            
        }
    }
}
