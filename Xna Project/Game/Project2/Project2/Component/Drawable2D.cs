using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;
using Project2.Component_Managers;

namespace Project2.Component
{
    class Drawable2D:Component
    {
        public Texture2D image { get; protected set; }
        Vector2 centerPos;
        public Color color;
        public static Component_Managers.SpriteManager spriteManager;
        public Byte alpha { get; set; }

        public Drawable2D(Game game, Entity.Entity parent,Texture2D image, Color color, Byte alpha = 255) 
            :base(game,parent)
        {
            this.image = image;
            centerPos.X = image.Width / 2.0f;
            centerPos.Y = image.Height / 2.0f;
            spriteManager.addSprite(Draw);
            this.color = color;
            this.alpha = alpha;
        }

        public static void init(Game game, SpriteFont font)
        {
            spriteManager = new Component_Managers.SpriteManager(game, font);
        }

        public override void cleanUp()
        {
            spriteManager.removeSprite(Draw);
            base.cleanUp();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            color.A = alpha;
            Spatial2D sp = getComponent<Spatial2D>();
            spriteBatch.Draw(image,sp.position , null, color, sp.rotation, centerPos,sp.scale, SpriteEffects.None,0);
        }
    }
}
