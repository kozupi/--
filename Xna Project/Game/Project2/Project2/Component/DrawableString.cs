using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;

namespace Project2.Component
{
    class DrawableString:Component
    {
        public String text { get; set; }
        public Color color { get; set; }
        public static Component_Managers.SpriteManager spriteManager;

        public DrawableString(Game game, Entity.Entity parent, String text, Color color)
            : base(game, parent)
        {
            this.text = text;
            this.color = color;
            spriteManager.addSprite(draw);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteManager.font, text, getComponent<Spatial2D>().position, color);
        }

        public static void init(Component_Managers.SpriteManager spriteManager)
        {
            DrawableString.spriteManager = spriteManager;
        }
    }
}
